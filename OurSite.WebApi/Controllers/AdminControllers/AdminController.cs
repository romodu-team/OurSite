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
                    return JsonStatusResponse.NotFound("اطلاعات کاربری اشتباه است");
                var role = await roleService.GetAdminRole(admin.Id);

                var token = authenticationHelper.GenerateAdminToken(admin, role, 3);
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success(new { Token = token, Expire = 3, UserId = admin.Id, admin.FirstName, admin.LastName }, "ورود با موفقیت انجام شد");
            }
            return JsonStatusResponse.Error("مشکلی در اطلاعات ارسالی وجود دارد");
        }
        #endregion

        #region singup
        /// <summary>
        ///  API for Register new Admin by system administrator {Get request from body}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] ReqRegisterAdminDto req)
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
        #endregion

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
                    return JsonStatusResponse.Success("رمز عبور با موفقیت تغییر کرد");
                return JsonStatusResponse.Error("عملیات با شکست مواجه شد");
            }
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
                    return JsonStatusResponse.Success("ایمیل بازنشانی رمز عبور با موفقیت ارسال شد");
                case ResLoginDto.IncorrectData:
                    return JsonStatusResponse.NotFound("حساب کاربری یافت نشد");

                default:
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
                            return JsonStatusResponse.Success("پنل کاربری شما با موفقیت ویرایش شد.");
                        case ResUpdate.Error:
                            return JsonStatusResponse.Error("خطا در هنگام انجام عملیات");
                        case ResUpdate.NotFound:
                            return JsonStatusResponse.NotFound("ارتباط شما با سرور قطع شده است");
                        default:
                            return JsonStatusResponse.Error("عملیات با شکست مواجه شد");
                    }
                }

                return JsonStatusResponse.Error("فیلدهای وارد شده؛ اشتباه است");
            }

            return JsonStatusResponse.NotFound("توکن شما نامعتبر است . مجدد وارد شوید");

        }
        #endregion




    }


}