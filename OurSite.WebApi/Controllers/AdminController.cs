using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurSite.Core.DTOs;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Utilities;

namespace OurSite.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService adminService;
        private readonly IUserService userService;
        public AdminController(IAdminService adminService, IUserService userService)
        {
            this.adminService = adminService;
            this.userService = userService;
        }
        [HttpPost("login-Admin")]
        public async Task<IActionResult> LoginAdmin(ReqLoginDto reqLogin)
        {
            var admin = await adminService.Login(reqLogin);
            if (admin == null)
                return JsonStatusResponse.NotFound("اطلاعات کاربری اشتباه است");
            var role = await adminService.GetAdminRole(admin.Id);

            var token = AuthenticationHelper.GenrateAdminToken(admin,role,3);
            HttpContext.Response.StatusCode = 200;
            return JsonStatusResponse.Success(new { Token = token, Expire = 3, UserId = admin.Id, FirstName = admin.FirstName, LastName = admin.LastName }, "ورود با موفقیت انجام شد");
        }
        [HttpGet("change-user-status/{userId}")]
        public async Task<IActionResult> ChangeUserStatus([FromRoute]long userId)
        {
            var res = await userService.ChangeUserStatus(userId);
            if (res)
                return JsonStatusResponse.Success("وضعیت کاربر تغییر کرد");
            return JsonStatusResponse.Error("عملیات نا موفق بود");
        }
    }
}
