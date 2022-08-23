using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OurSite.Core.DTOs.RoleDtos;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Utilities;
using OurSite.DataLayer.Entities.Access;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OurSite.WebApi.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    public class AdminRoleManagementController : Controller
    {
        #region Constructor
        private readonly IRoleService roleService;
        private readonly IAdminService _adminService;
        public AdminRoleManagementController(IAdminService adminService, IRoleService roleService)
        {
            this.roleService = roleService;
            _adminService = adminService;

        }
        #endregion

        #region Add new role
        /// <summary>
        ///  API for Add new role by system administrator {Get request from body}
        /// </summary>
        /// <param name="title"></param>
        /// <remarks>Role id should be null</remarks>
        /// <returns></returns>
        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromQuery] string title,[FromQuery]string name)
        {
            var res = await roleService.AddRole(title,name);
            switch (res)
            {
                case ResAddRole.Success:
                    HttpContext.Response.StatusCode=201;
                    return JsonStatusResponse.Success("نقش با موفقیت اضافه شد");
                case ResAddRole.Faild:
                     HttpContext.Response.StatusCode=500;
                    return JsonStatusResponse.Error("اضافه کردن نقش جدید با خطا مواجه شد. مجددا تلاش نمایید.");

                case ResAddRole.InvalidInput:
                    HttpContext.Response.StatusCode=400;
                    return JsonStatusResponse.InvalidInput();
                case ResAddRole.Exist:
                    HttpContext.Response.StatusCode=409;
                    return JsonStatusResponse.Error("این نقش قبلا اضافه شده است.");
                default:
                    HttpContext.Response.StatusCode=500;
                    return JsonStatusResponse.Error("عملیات با خطا مواجه شد.");
            }
        }
        #endregion

        #region Delete a role
        /// <summary>
        ///  API for Delete role by system administrator {Get request from Query}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete("delete-role")]
        public async Task<IActionResult> RemoveRole([FromQuery] long RoleId)
        {
            var remove = await roleService.RemoveRole(RoleId);
            switch (remove)
            {
                case ResRole.Success:
                    return JsonStatusResponse.Success("The project has been deleted successfully");
                case ResRole.Error:
                    return JsonStatusResponse.Error("Project delete failed. Try again later.");
                case ResRole.NotFound:
                    return JsonStatusResponse.NotFound("Project not Found");
                default:
                    return JsonStatusResponse.Error("An error has occurred. Try again later.");
            }
        }
        #endregion

        #region Update role
        /// <summary>
        ///  API for Update role by system administrator {Get request from body}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("update-role")]
        public async Task<IActionResult> UpdateRole([FromBody] ReqUpdateRoleDto reqUpdate)
        {
            if (ModelState.IsValid)
            {
                var res = await roleService.UpdateRole(reqUpdate);
                switch (res)
                {
                    case ReqUpdateRoleDto.ResUpdateRole.Success:
                        return JsonStatusResponse.Success("نقش با موفقیت بروزرسانی شد");
                    case ReqUpdateRoleDto.ResUpdateRole.Error:
                        return JsonStatusResponse.Error("عملیات با شکست مواجه شد");
                    case ReqUpdateRoleDto.ResUpdateRole.NotFound:
                        return JsonStatusResponse.NotFound("نقش مورد نظر پیدا نشد");
                    case ReqUpdateRoleDto.ResUpdateRole.Exist:
                        return JsonStatusResponse.NotFound("نام نقش تکراری است");
                    default:
                        return JsonStatusResponse.Error("عملیات با شکست مواجه شد");
                }
            }
            return JsonStatusResponse.Error("فیلد های اجباری باید پر شوند");
        }
        #endregion

        #region Found role by role id
        /// <summary>
        ///  API for find role by role id {Get request from route}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("view-Role/{RoleId}")]
        public async Task<IActionResult> GetRoleById([FromRoute] long RoleId)
        {
            var role = await roleService.GetRoleById(RoleId);
            if (role is null)
                return JsonStatusResponse.NotFound("نقش مورد نظر پیدا نشد");
            return JsonStatusResponse.Success(role, "موفق");
        }
        #endregion

        #region role list
        /// <summary>
        ///  API for Get list of all{Get request from Query}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("view-Roles")]
        public async Task<IActionResult> GetAllRoles([FromQuery] ReqFilterRolesDto filter)
        {
            var roles = await roleService.GetActiveRoles(filter);
            if (roles is null)
                return JsonStatusResponse.NotFound("هیچ نقشی پیدا نشد");
            return JsonStatusResponse.Success(roles, "موفق");
        }
        #endregion

        #region Update admin role(delete before account in role and add new one)
        /// <summary>
        /// If the admin already has a role, her role will be updated, otherwise a new role will be assigned to her
        /// send parameters from query
        /// result can be : Success,NotFound(role or admin),Error
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        [HttpPut("Update-admin-role")]
        public async Task<IActionResult> UpdateAdminRole(long adminId,long RoleId)
        {
            var res = await _adminService.UpdateAdminRole(adminId, RoleId);
            switch (res)
            {
                case Core.DTOs.AdminDtos.ResUpdate.Success:
                    return JsonStatusResponse.Success("role has been updated successfully ");
                case Core.DTOs.AdminDtos.ResUpdate.NotFound:
                    return JsonStatusResponse.NotFound("admin not found");
               
                case Core.DTOs.AdminDtos.ResUpdate.Error:
                    return JsonStatusResponse.Error("role has not been updated ");

                case Core.DTOs.AdminDtos.ResUpdate.RoleNotFound:
                    return JsonStatusResponse.NotFound("role not found");

                default:
                    return JsonStatusResponse.Error("role has not been updated ");

            }
        }
        #endregion

        #region get all permission
        /// <summary>
        /// Returns a list of all available permissions for which the selected permissions for this role are specified with isCheck=true
        /// result can be Success with permission list , NotFound
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("get-all-permissions/{roleId}")]
        public async Task<IActionResult> GetAllPermissions([FromRoute]long roleId)
        {
            var permissions =await roleService.GetAllPermission(roleId);
            if (permissions.Any())
                return JsonStatusResponse.Success(permissions, "success");
            return JsonStatusResponse.NotFound("No permissons found");
        }
        #endregion

        #region Update role permissions(delete before perrmissions and add new permissions)
        /// <summary>
        /// parameters: role id and The ID list of the permissions selected for this role
        /// Deletes all existing permissions and registers new permissions
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("update-permissions-role")]
        public async Task<IActionResult> UpdatePermissionRole([FromBody]ReqUpdatePermissionRole request)
        {
            var res =await roleService.UpdatePermissionRole(request);
            switch (res)
            {
                case resUpdatePermissionRole.Success:
                    return JsonStatusResponse.Success("permissions of role has been updated successfully");
                case resUpdatePermissionRole.RoleNotFound:
                    return JsonStatusResponse.NotFound("role not found");
                case resUpdatePermissionRole.permissionNotFound:
                    return JsonStatusResponse.NotFound("one of the permissions not found");
                case resUpdatePermissionRole.Error:
                    return JsonStatusResponse.Error("server error");
                default:
                    return JsonStatusResponse.Error("server error");
            }
        }
        #endregion
    }
}

