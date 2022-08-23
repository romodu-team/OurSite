using System.Net.Http;
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
using System.Security.Claims;

namespace OurSite.WebApi.Controllers.AdminControllers
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

        #region login
        /// <summary>
        ///  API for Login Admin into system {Get request from body}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login-Admin")]
        public async Task<IActionResult> LoginAdmin([FromBody] ReqLoginDto reqLogin)
        {
            AuthenticationHelper authenticationHelper = new AuthenticationHelper(roleService);
            if (ModelState.IsValid)
            {
                var admin = await adminService.Login(reqLogin);
                if (admin is null)
                {
                    HttpContext.Response.StatusCode = 404;
                    return JsonStatusResponse.NotFound("there isn't any admin mieh this information");
                }
                   
                var role = await roleService.GetAdminRole(admin.Id);

                var token =await authenticationHelper.GenerateAdminToken(admin, role, 3);
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success(new { Token = token, Expire = 3, UserId = admin.Id, admin.FirstName, admin.LastName }, "Success");
            }
            HttpContext.Response.StatusCode = 400;
            return JsonStatusResponse.InvalidInput();
        }
        #endregion






        //[Authorize]
        //[HttpPost("logout")]
        //public async Task<IActionResult> Logout()
        //{
        //    var Id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        //}


        #region Reset Password
        /// <summary>
        ///  API for Reset Password Admin {Get request from body}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ReqResetPassword request)
        {
            if (ModelState.IsValid)
            {
                var res = await adminService.ResetPassword(request);
                if (res)
                {
                    HttpContext.Response.StatusCode = 200;
                    return JsonStatusResponse.Success("رمز عبور با موفقیت تغییر کرد");
                }
                HttpContext.Response.StatusCode = 500;
                return JsonStatusResponse.Error("عملیات با شکست مواجه شد");
            }
            HttpContext.Response.StatusCode = 200;
            return JsonStatusResponse.Error("مشکلی در اطلاعات ارسالی وجود دارد");
        }
        #endregion

        #region Send Reset Password to admin Email
        /// <summary>
        ///  API for send reset password for Admin Email {Get request from body}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("SendEmail-ResetUserPass")]
        public async Task<IActionResult> SendResetPassLink([FromBody] string EmailOrUserName)
        {
            var res = await adminService.SendResetPassEmail(EmailOrUserName);
            switch (res)
            {
                case ResLoginDto.Success:
                HttpContext.Response.StatusCode = 200;
                    return JsonStatusResponse.Success("ایمیل بازنشانی رمز عبور با موفقیت ارسال شد");
                case ResLoginDto.IncorrectData:
                HttpContext.Response.StatusCode = 404;
                    return JsonStatusResponse.NotFound("حساب کاربری یافت نشد");

                default:
                HttpContext.Response.StatusCode = 500;
                    return JsonStatusResponse.Error("عملیات با شکست مواجه شد");
            }

        }
        #endregion

        #region Update admin by self
        /// <summary>
        ///  API for Update admin profile by self{Get request from form}
        /// </summary>
        /// <remarks>The file size of the profile image must be less than 3 MB</remarks>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut("update-admin-profile")]
        public async Task<IActionResult> UpdateAdminBySelf([FromForm] ReqUpdateAdminDto req)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    var Adminid = User.FindFirst(ClaimTypes.Sid);
                    var res = await adminService.UpdateAdmin(req, Convert.ToInt64(Adminid.Value));
                    if (req.ProfilePhoto != null)
                    {
                        var resProfilePhoto = await adminService.ProfilePhotoUpload(req.ProfilePhoto, Convert.ToInt64(Adminid.Value));
                        switch (resProfilePhoto)
                        {
                            case resFileUploader.Failure:
                                return JsonStatusResponse.Error("اپلود تصویر پروفایل با مشکل مواجه شد");

                            case resFileUploader.ToBig:
                                return JsonStatusResponse.Error("حجم تصویر پروفایل انتخابی بیش از سقف مجاز است");

                            case resFileUploader.NoContent:
                                return JsonStatusResponse.Error("تصویر پروفایل خالی است");
                            case resFileUploader.InvalidExtention:
                                return JsonStatusResponse.Error("پسوند فایل انتخابی مجاز نیست");
                            default:
                                break;
                        }
                    }
                    switch (res)
                    {
                        
                        case ResUpdate.Success:
                        HttpContext.Response.StatusCode = 200;
                            return JsonStatusResponse.Success("پنل کاربری شما با موفقیت ویرایش شد.");
                        case ResUpdate.Error:
                        HttpContext.Response.StatusCode = 500;
                            return JsonStatusResponse.Error("خطا در هنگام انجام عملیات");
                        case ResUpdate.NotFound:
                        HttpContext.Response.StatusCode = 404;
                            return JsonStatusResponse.NotFound("ارتباط شما با سرور قطع شده است");
                        default:
                        HttpContext.Response.StatusCode = 400;
                            return JsonStatusResponse.Error("عملیات با شکست مواجه شد");
                    }
                }
                HttpContext.Response.StatusCode = 400;
                return JsonStatusResponse.Error("فیلدهای وارد شده؛ اشتباه است");
            }
            HttpContext.Response.StatusCode = 401;
            return JsonStatusResponse.NotFound("توکن شما نامعتبر است . مجدد وارد شوید");

        }
        #endregion




    }


}