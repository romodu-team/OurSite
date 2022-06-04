using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurSite.Core.DTOs;
using OurSite.Core.DTOs.AdminDtos;
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
        public AdminController(IAdminService adminService, IUserService userService)
        {
            this.adminService = adminService;
            this.userService = userService;
        }
        #endregion

        [HttpPost("login-Admin")]
        public async Task<IActionResult> LoginAdmin([FromBody]ReqLoginDto reqLogin)
        {
            var admin = await adminService.Login(reqLogin);
            if (admin == null)
                return JsonStatusResponse.NotFound("اطلاعات کاربری اشتباه است");
            var role = await adminService.GetAdminRole(admin.Id);

            var token = AuthenticationHelper.GenerateAdminToken(admin,role,3);
            HttpContext.Response.StatusCode = 200;
            return JsonStatusResponse.Success(new { Token = token, Expire = 3, UserId = admin.Id, FirstName = admin.FirstName, LastName = admin.LastName }, "ورود با موفقیت انجام شد");
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
            var res =await adminService.RegisterAdmin(req);
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
    }
}
