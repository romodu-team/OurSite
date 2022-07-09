using Microsoft.EntityFrameworkCore;
using OurSite.Core.DTOs.RoleDtos;
using OurSite.Core.Services.Interfaces;
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
        public RoleService(IGenericReopsitories<Role> RoleRepository, IGenericReopsitories<AccounInRole> accounInRoleRepository)
        {
            this.RoleRepository = RoleRepository;
            this.accounInRoleRepository = accounInRoleRepository;
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
                        CreateDate = DateTime.Now,
                        LastUpdate = DateTime.Now,
                        IsRemove = false,
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
        public async Task<List<RoleDto>> GetActiveRoles()        //
        {
            return await RoleRepository.GetAllEntity().Select(r => new RoleDto { Name = r.Name, Title = r.Title }).ToListAsync();
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
        public async Task<bool> RemoveRole(long RoleId)
        {
            var role = await RoleRepository.GetEntity(RoleId);
            if (role is null)
                return false;
            role.IsRemove = true;
            role.LastUpdate = DateTime.Now;
            try
            {
                RoleRepository.UpDateEntity(role);
                await RoleRepository.SaveEntity();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        #endregion

        public async Task<ResUpdateRole> UpdateRole(ReqUpdateRoleDto reqUpdate)
        {
            var role=await RoleRepository.GetEntity(reqUpdate.RoleID);
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

        public async Task<Role> GetAdminRole(long adminId)
        {
            var role = await accounInRoleRepository.GetAllEntity().Include(a => a.Role).SingleOrDefaultAsync(r => r.AdminId == adminId && r.IsRemove == false);
            return role.Role;
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

        public async Task<ResDeleAdminRole> DeleteAdminRole(long adminId)
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
                    return ResDeleAdminRole.Success;
                }
                catch (Exception)
                {

                    return ResDeleAdminRole.Faild;
                }
                
            }
            return ResDeleAdminRole.NotExist;
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

        public async Task<AccounInRole> GetAdminInRole(long adminId)
        {
            return await accounInRoleRepository.GetAllEntity().SingleOrDefaultAsync(a => a.AdminId == adminId && a.IsRemove == false);
        }

        #endregion

    }
}
