using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurSite.Core.DTOs;
using OurSite.Core.DTOs.AdminDtos;
using OurSite.Core.DTOs.RoleDtos;
using OurSite.Core.DTOs.UserDtos;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Utilities;
using System.Linq;

namespace OurSite.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        #region Constructor
        private readonly IAdminService adminService;
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        public AdminController(IAdminService adminService, IUserService userService, IRoleService roleService)
        {
            this.adminService = adminService;
            this.userService = userService;
            this.roleService = roleService;
        }
        #endregion

        [HttpPost("login-Admin")]
        public async Task<IActionResult> LoginAdmin([FromBody]ReqLoginDto reqLogin)
        {
            if (ModelState.IsValid)
            {
                var admin = await adminService.Login(reqLogin);
                if (admin is null)
                    return JsonStatusResponse.NotFound("اطلاعات کاربری اشتباه است");
                var role = await roleService.GetAdminRole(admin.Id);

                var token = AuthenticationHelper.GenerateAdminToken(admin, role, 3);
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success(new { Token = token, Expire = 3, UserId = admin.Id, FirstName = admin.FirstName, LastName = admin.LastName }, "ورود با موفقیت انجام شد");
            }
            return JsonStatusResponse.Error("مشکلی در اطلاعات ارسالی وجود دارد");
        }

        [Authorize(Roles = "General Manager,Admin")]
        [HttpGet("change-user-status/{userId}")]
        public async Task<IActionResult> ChangeUserStatus([FromRoute]long userId)
        {
            var res = await userService.ChangeUserStatus(userId);
            if (res)
                return JsonStatusResponse.Success("وضعیت کاربر تغییر کرد");
            return JsonStatusResponse.Error("عملیات نا موفق بود");
        }

        [Authorize(Roles ="General Manager")]
        [HttpPost("Update-Admin")]
        public async Task<IActionResult> UpdateAdmin([FromBody]ReqUpdateAdminDto req)
        {
      
                var res = await adminService.UpdateAdmin(req);
                switch (res)
                {
                    case resUpdateAdmin.Success:
                        return JsonStatusResponse.Success("با موفقیت ویرایش شد");
                    case resUpdateAdmin.NotFound:
                        return JsonStatusResponse.NotFound("حساب کاربری پیدا نشد");
                    case resUpdateAdmin.Error:
                        return JsonStatusResponse.Error("خطا در هنگام انجام عملیات");
                    default:
                        return JsonStatusResponse.Error("عملیات با شکست مواجه شد");
                }

        }

        [Authorize(Roles = "General Manager")]
        [HttpDelete("delete-admin")]
        public async Task<IActionResult> DeleteAdmin([FromQuery] long adminId)
        {
           var res= await adminService.DeleteAdmin(adminId);
            if (res)
                return JsonStatusResponse.Success("با موفقیت حذف شد");
            return JsonStatusResponse.Error("عملیات با شکست مواجه شد");
        }
        [Authorize(Roles = "General Manager")]
        [HttpGet("view-admin/{adminId}")]
        public async Task<IActionResult> GetAdmin([FromRoute]long adminId)
        {
            var res =await adminService.GetAdminById(adminId);
            if (res != null)
                return JsonStatusResponse.Success(res, "موفق");
            HttpContext.Response.StatusCode = 404;
            return JsonStatusResponse.NotFound("پیدا نشد");
        }
        [Authorize(Roles = "General Manager")]
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody]ReqSingupUserDto req)
        {
            if (ModelState.IsValid)
            {
                var res = await adminService.RegisterAdmin(req);
                switch (res)
                {
                    case RessingupDto.success:
                        HttpContext.Response.StatusCode = 201;
                        return JsonStatusResponse.Success("اطلاعات با موفقیت ثبت شد");
                    case RessingupDto.Exist:
                        return JsonStatusResponse.Error("نام کاربری یا ایمیل تکراری است");
                    default:
                        return JsonStatusResponse.Error("عملیات با شکست مواجه شد");
                }
            }
            return JsonStatusResponse.Error("مشکلی در اطلاعات ارسالی وجود دارد");
           
        }

        #region Reset Password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ReqResetPassword request)
        {
            if (ModelState.IsValid)
            {
                var res = await adminService.ResetPassword(request);
                if (res)
                    return JsonStatusResponse.Success("رمز عبور با موفقیت تغییر کرد");
                return JsonStatusResponse.Error("عملیات با شکست مواجه شد");
            }
            return JsonStatusResponse.Error("مشکلی در اطلاعات ارسالی وجود دارد");
        }
        #endregion

        #region Send Reset Password Email
        [HttpPost("SendEmail-ResetUserPass")]
        public async Task<IActionResult> SendResetPassLink([FromBody] string EmailOrUserName)
        {
            var res = await adminService.SendResetPassEmail(EmailOrUserName);
            switch (res)
            {
                case ResLoginDto.Success:
                    return JsonStatusResponse.Success("ایمیل بازنشانی رمز عبور با موفقیت ارسال شد");
                case ResLoginDto.IncorrectData:
                    return JsonStatusResponse.NotFound("حساب کاربری یافت نشد");

                default:
                    return JsonStatusResponse.Error("عملیات با شکست مواجه شد");
            }

        }
        #endregion

        #region Role Management
        [Authorize(Roles = "General Manager")]
        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromBody]RoleDto role)
        {
            var res =await roleService.AddRole(role);
            if (res)
                return JsonStatusResponse.Success("نقش با موفقیت اضافه شد");
            return JsonStatusResponse.Error("عملیات با شکست مواجه شد");
        }

        [Authorize(Roles = "General Manager")]
        [HttpDelete("delete-role")]
        public async Task<IActionResult> RemoveRole([FromBody]long RoleId)
        {
            var res =await roleService.RemoveRole(RoleId);
            if(res)
                return JsonStatusResponse.Success("نقش با موفقیت حذف شد");
            return JsonStatusResponse.Error("عملیات با شکست مواجه شد");
        }

        [Authorize(Roles = "General Manager")]
        [HttpPut("update-role")]
        public async Task<IActionResult> UpdateRole(ReqUpdateRoleDto reqUpdate)
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
                default:
                    return JsonStatusResponse.Error("عملیات با شکست مواجه شد");
            }
        }

        [Authorize(Roles = "General Manager")]
        [HttpGet("view-Role/{RoleId}")]
        public async Task<IActionResult> GetRoleById([FromRoute]long RoleId)
        {
            var role = await roleService.GetRoleById(RoleId);
            if (role is null)
                return JsonStatusResponse.NotFound("نقش مورد نظر پیدا نشد");
            return JsonStatusResponse.Success(role, "موفق");
        }

        [Authorize(Roles = "General Manager")]
        [HttpGet("view-Roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await roleService.GetActiveRoles();
            if (roles is null)
                return JsonStatusResponse.NotFound("هیچ نقشی پیدا نشد");
            return JsonStatusResponse.Success(roles, "موفق");
        }
        #endregion
        [HttpGet("view-user/{userid}")]
        public async Task<IActionResult> ViewUser(long userid)
        {
           var user = await userService.ViewUser(userid);
           if (user is not null)
                return JsonStatusResponse.Success(message:"success",ReturnData:user);


            return JsonStatusResponse.Error(message: "کاربر پیدا نشد");
        }


        [HttpGet("view-all-users")]
        public async Task<IActionResult> GetAllUser()
        {
            var users = await userService.GetAlluser();
            if (users.Any())
            {
                return JsonStatusResponse.Success(message: "موفق" , ReturnData: users);
            }
            return JsonStatusResponse.NotFound(message: "کاربری یافت نشد");
            
        }

    }


}
