using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using OurSite.Core.DTOs.Paging;
using OurSite.Core.DTOs.RoleDtos;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Utilities.Extentions.Paging;
using OurSite.DataLayer.Entities.Access;
using OurSite.DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OurSite.Core.DTOs.RoleDtos.ReqUpdateRoleDto;

namespace OurSite.Core.Services.Repositories
{
    public class RoleService : IRoleService
    {
        #region Constructor
        private IGenericReopsitories<Role> RoleRepository;
        private IGenericReopsitories<AccounInRole> accounInRoleRepository;
        private IGenericReopsitories<RolePermission> RolePermissionRepository;
        private IGenericReopsitories<Permission> PermissionRepository;
        public RoleService(IGenericReopsitories<Permission> PermissionRepository, IGenericReopsitories<RolePermission> RolePermissionRepository,IGenericReopsitories<Role> RoleRepository, IGenericReopsitories<AccounInRole> accounInRoleRepository)
        {
            this.RoleRepository = RoleRepository;
            this.accounInRoleRepository = accounInRoleRepository;
            this.RolePermissionRepository=RolePermissionRepository;
            this.PermissionRepository=PermissionRepository;
        }
        #endregion

        #region Add new role for admin
        public async Task<ResAddRole> AddRole(RoleDto role)
        {

            if (!string.IsNullOrWhiteSpace(role.Name) && !string.IsNullOrWhiteSpace(role.Title))
            {
                if (!RoleRepository.GetAllEntity().Any(r => (r.Name == role.Name || r.Title == role.Title) && r.IsRemove == false))
                {
                    Role newRole = new Role()
                    {
                        Name = role.Name,
                        Title = role.Title
                    };
                    try
                    {

                        await RoleRepository.AddEntity(newRole);
                        await RoleRepository.SaveEntity();
                        return ResAddRole.Success;
                    }
                    catch (Exception)
                    {

                        return ResAddRole.Faild;
                    }
                }
                return ResAddRole.Exist;   
                

            }
            return ResAddRole.InvalidInput;

        }

        #endregion

        #region Get roles
        public async Task<ResRoleFilterDto> GetActiveRoles(ReqFilterRolesDto filter)        //
        {
            var RolesQuery = RoleRepository.GetAllEntity().AsQueryable();
            switch (filter.RolesOrderBy)
            {
                case RolesOrderBy.NameAsc:
                    RolesQuery = RolesQuery.OrderBy(u => u.Title);
                    break;
                case RolesOrderBy.NameDec:
                    RolesQuery = RolesQuery.OrderByDescending(u => u.Title);
                    break;
                case RolesOrderBy.CreateDateAsc:
                    RolesQuery = RolesQuery.OrderBy(u => u.CreateDate);
                    break;
                case RolesOrderBy.CreateDateDec:
                    RolesQuery = RolesQuery.OrderByDescending(u => u.CreateDate);
                    break;
                default:
                    break;
            }
            switch (filter.RolesRemoveFilter)
            {
                case RolesRemoveFilter.Deleted:
                    RolesQuery = RolesQuery.Where(u => u.IsRemove == true);
                    break;
                case RolesRemoveFilter.NotDeleted:
                    RolesQuery = RolesQuery.Where(u => u.IsRemove == false);
                    break;
                case RolesRemoveFilter.All:
                    break;
                default:
                    break;
            }
            
            if (!string.IsNullOrWhiteSpace(filter.RoleTitleSearchKey))
                RolesQuery = RolesQuery.Where(u => u.Title.Contains(filter.RoleTitleSearchKey));
            if (!string.IsNullOrWhiteSpace(filter.RoleNameSearchKey))
                RolesQuery = RolesQuery.Where(u => u.Name.Contains(filter.RoleNameSearchKey));

            var count = (int)Math.Ceiling(RolesQuery.Count() / (double)filter.TakeEntity);
            var pager = Pager.Build(count, filter.PageId, filter.TakeEntity);
            var list = await RolesQuery.Paging(pager).Select(r => new RoleDto { Name = r.Name, Title = r.Title }).ToListAsync();    //use the genric interface options and save values in variable

            var result = new ResRoleFilterDto();
            result.SetPaging(pager);
            return result.SetRoles(list);
            //return await RoleRepository.GetAllEntity().Select(r => new RoleDto { Name = r.Name, Title = r.Title }).ToListAsync();
        }
        #endregion

        #region founder role by id
        public async Task<RoleDto> GetRoleById(long RoleId)
        {
            var role = await RoleRepository.GetEntity(RoleId);
            if (role is null)
                return null;
            return new RoleDto { Name = role.Name, Title = role.Title };
        }
        #endregion

        #region Delete role
        public async Task<ResRole> RemoveRole(long RoleId)
        {
            var IsRemove = await RoleRepository.DeleteEntity(RoleId);
            if (IsRemove is true)
            {
                //delete permission of this role
                var permissions =await GetSelectedPermissionOfRole(RoleId);
                var res = await DeletePermissionOfRole(permissions);

                //delele roles of admin
                var resDeleteAdminRoles= await DeleteAdminRoleByRoleId(RoleId);
                await RoleRepository.SaveEntity();
                return ResRole.Success;
            }
            else
            {
                return ResRole.Error;

            }
            return ResRole.NotFound;
        }
        #endregion

        public async Task<ResUpdateRole> UpdateRole(ReqUpdateRoleDto reqUpdate)
        {
            if (!RoleRepository.GetAllEntity().Any(r => r.Title == reqUpdate.RoleTitle))
            {
                var role = await RoleRepository.GetEntity(reqUpdate.RoleID);
                if (role is null)
                    return ResUpdateRole.NotFound;
                role.Title = reqUpdate.RoleTitle;
                role.LastUpdate = DateTime.Now;

                try
                {
                    RoleRepository.UpDateEntity(role);
                    await RoleRepository.SaveEntity();
                    return ResUpdateRole.Success;
                }
                catch (Exception)
                {
                    return ResUpdateRole.Error;
                }
            }
            return ResUpdateRole.Exist;
        }

        public async Task<Role> GetAdminRole(long adminId)
        {
            var role = await accounInRoleRepository.GetAllEntity().Include(a => a.Role).SingleOrDefaultAsync(r => r.AdminId == adminId && r.IsRemove == false);
            if(role is not null)
                return role.Role;
            return null;
        }

        #region Dispose
        public void Dispose()
        {
            RoleRepository.Dispose();
        }

        public async Task<bool> AddRoleToAdmin(AccounInRole accounInRole)
        {
            try
            {

                await accounInRoleRepository.AddEntity(accounInRole);
                await accounInRoleRepository.SaveEntity();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
           
        }
        public async  Task<ResDeleteAdminRole> DeleteAdminRoleByRoleId(long RoleId)
        {
            var allAdminInRoles = await accounInRoleRepository.GetAllEntity().Where(a=>a.RoleId==RoleId).ToListAsync();
            foreach (var AdminInRole in allAdminInRoles)
            {
                await accounInRoleRepository.DeleteEntity(AdminInRole.Id);
            }
            try
            {
                await accounInRoleRepository.SaveEntity();
                return ResDeleteAdminRole.Success;
            }
            catch (System.Exception)
            {
                
                return ResDeleteAdminRole.Faild;
            }
            
        }
        public async Task<ResDeleteAdminRole> DeleteAdminRole(long adminId)
        {
            var AdminInRole= await accounInRoleRepository.GetAllEntity().SingleOrDefaultAsync(r => r.AdminId == adminId && r.IsRemove == false);
            if (AdminInRole != null)
            {
                AdminInRole.IsRemove = true;
                AdminInRole.LastUpdate = DateTime.Now;
                try
                {
                    await accounInRoleRepository.DeleteEntity(AdminInRole.Id);
                    await accounInRoleRepository.SaveEntity();
                    return ResDeleteAdminRole.Success;
                }
                catch (Exception)
                {

                    return ResDeleteAdminRole.Faild;
                }
                
            }
            return ResDeleteAdminRole.NotExist;
        }

        public async Task<Role> GetRoleByName(string RoleName)
        {
            var role=await RoleRepository.GetAllEntity().SingleOrDefaultAsync(r => r.Name == RoleName && r.IsRemove == false);
            if (role is null)
                return null;
            else
                return role;
        }

        public async Task<bool> UpdateAdminRole(AccounInRole accounInRole)
        {
            accounInRole.LastUpdate = DateTime.Now;

            try
            {
                accounInRoleRepository.UpDateEntity(accounInRole);
                await accounInRoleRepository.SaveEntity();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
           

        }
        public async Task<bool> DeletePermissionOfRole(List<Permission> permissions)
        {
            foreach (var permission in permissions)
            {
                await RolePermissionRepository.RealDeleteEntity(permission.Id);

            }
            try
            {
                 await RolePermissionRepository.SaveEntity();
                 return true;
            }
            catch (System.Exception e)
            {
                
                return false;
            }
           
        }
        public async Task<AccounInRole> GetAdminInRole(long adminId)
        {
            return await accounInRoleRepository.GetAllEntity().SingleOrDefaultAsync(a => a.AdminId == adminId && a.IsRemove == false);
        }

        public async Task<List<Permission>> GetSelectedPermissionOfRole(long roleId)
        {
            var RolePermission=  await RolePermissionRepository.GetAllEntity().Include(u=>u.Permission).Select(r=> new Permission{Id=r.Permission.Id,PermissionName=r.Permission.PermissionName,PermissionTitle=r.Permission.PermissionTitle,CreateDate=r.Permission.CreateDate,LastUpdate=r.Permission.LastUpdate}).ToListAsync();
            return RolePermission;
        }


        public async Task<List<ResGetAllPermissions>> GetAllPermission(long roleId)
        {
            var SelectedPermissions =await RolePermissionRepository.GetAllEntity().Where(rp => rp.RolesId == roleId).Include(rp => rp.Permission).Select(p => new ResGetAllPermissions {IsCheck=true, PermissionId = p.Permission.Id, PermissionName = p.Permission.PermissionName, PermissionTitle = p.Permission.PermissionTitle, ParentId = p.Permission.ParentId }).ToListAsync();
            var allPermissions =await PermissionRepository.GetAllEntity().ToListAsync();
            foreach (var item in allPermissions)
            {
                if (!SelectedPermissions.Any(p => p.PermissionId == item.Id))
                {
                    SelectedPermissions.Add(new ResGetAllPermissions { IsCheck = false, PermissionId = item.Id, PermissionName = item.PermissionName, PermissionTitle = item.PermissionTitle, ParentId = item.ParentId });
                }
            }
            return SelectedPermissions;
        }

        public async Task<resUpdatePermissionRole> UpdatePermissionRole(ReqUpdatePermissionRole request)
        {
            //check role is exist
            var roleExist =await RoleRepository.GetEntity(request.RoleId);
            if (roleExist is null)
                return resUpdatePermissionRole.RoleNotFound;
            if(request.PermissionsId is not null)
            {
                // delete all before permissions
                var allExistRolePermission = await RolePermissionRepository.GetAllEntity().Where(r => r.RolesId == request.RoleId).ToListAsync();
                foreach (var item in allExistRolePermission)
                {
                    await RolePermissionRepository.RealDeleteEntity(item.Id);
                }
                await RoleRepository.SaveEntity();
                // add new permissions
                foreach (var permission in request.PermissionsId)
                {
                    if (PermissionRepository.GetAllEntity().Any(p => p.Id == permission))
                    {
                        var rolePermission = new RolePermission()
                        {
                            PermissionId = permission,
                            RolesId = request.RoleId
                        };
                        await RolePermissionRepository.AddEntity(rolePermission);
                    }
                    else
                        return resUpdatePermissionRole.permissionNotFound;
                }
                try
                {
                    await RolePermissionRepository.SaveEntity();
                    return resUpdatePermissionRole.Success;
                }
                catch (Exception)
                {

                    return resUpdatePermissionRole.Error;
                }
               
            }
            return resUpdatePermissionRole.permissionNotFound;

        }
        #endregion

    }
}
