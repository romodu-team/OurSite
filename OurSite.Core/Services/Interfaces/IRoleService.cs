using OurSite.Core.DTOs.RoleDtos;
using OurSite.DataLayer.Entities.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OurSite.Core.DTOs.RoleDtos.ReqUpdateRoleDto;

namespace OurSite.Core.Services.Interfaces
{
    public interface IRoleService:IDisposable
    {
        Task<ResAddRole> AddRole(RoleDto role);
        Task<ResRole> RemoveRole(long RoleId);
        Task<ResUpdateRole> UpdateRole(ReqUpdateRoleDto reqUpdate);
        Task<RoleDto> GetRoleById(long RoleId);
        Task<Role> GetRoleByName(string RoleName);
        Task<ResRoleFilterDto> GetActiveRoles(ReqFilterRolesDto filter);
        Task<Role> GetAdminRole(long adminId);

        Task<bool> AddRoleToAdmin(AccounInRole accounInRole);
        Task<ResDeleteAdminRole> DeleteAdminRole(long adminId);
        Task<ResDeleteAdminRole> DeleteAdminRoleByRoleId(long RoleId);
        Task<bool> UpdateAdminRole(AccounInRole accounInRole);
        Task<AccounInRole> GetAdminInRole(long adminId);

        Task<List<Permission>> GetSelectedPermissionOfRole(long roleId);
        Task<List<ResGetAllPermissions>> GetAllPermission(long roleId);
        Task<resUpdatePermissionRole> UpdatePermissionRole(ReqUpdatePermissionRole request);
        Task<bool> DeletePermissionOfRole(List<Permission> permissions);
    }
}
