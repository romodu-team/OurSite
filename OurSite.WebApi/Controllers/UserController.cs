using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OurSite.Core.DTOs;
using OurSite.Core.DTOs.AdminDtos;
using OurSite.Core.DTOs.UserDtos;
using OurSite.Core.Security;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Utilities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OurSite.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        #region constructor
        private IUserService userservice;

        public UserController(IUserService userService)
        {
            this.userservice = userService;

        }
        #endregion

        #region Login
        /// <summary>
        ///  API for login user into system {Get request from body}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] ReqLoginDto request)
        {
            if (ModelState.IsValid)
            {
                var res = await userservice.LoginUser(request);
                switch (res)
                {
                    case ResLoginDto.Success:

                        var user = await userservice.GetUserByUserPass(request.UserName, request.Password);
                        var token = AuthenticationHelper.GenerateUserToken(user, 3);
                        HttpContext.Response.StatusCode = 200;
                        return JsonStatusResponse.Success(new { Token = token, Expire = 3, UserId = user.Id, FirstName = user.FirstName, LastName = user.LastName }, "ورود با موفقیت انجام شد");
                    case ResLoginDto.IncorrectData:

                        return JsonStatusResponse.NotFound("نام کاربری یا رمز عبور اشتباه است");

                    case ResLoginDto.NotActived:
                        return JsonStatusResponse.Error("حساب کاربری شما فعال نیست");

                    case ResLoginDto.Error:
                        return JsonStatusResponse.Error("مشکلی در اطلاعات ارسالی وجود دارد");
                    default:
                        HttpContext.Response.StatusCode = 400;
                        return JsonStatusResponse.Error("عملیات با خطا مواجه شد");

                }
            }
            return JsonStatusResponse.Error("مشکلی در اطلاعات ارسالی وجود دارد");
        }
        #endregion

        #region Reset Password
        /// <summary>
        ///  API for Reset password User Panel {Get request from body}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ReqResetPassword request)
        {
            if (ModelState.IsValid)
            {
                var res = await userservice.ResetPassword(request);
                if (res)
                    return JsonStatusResponse.Success("رمز عبور با موفقیت تغییر کرد");
                return JsonStatusResponse.Error("عملیات با شکست مواجه شد");
            }
            return JsonStatusResponse.Error("مشکلی در اطلاعات ارسالی وجود دارد");
        }
        #endregion

        #region Send Reset Password Email
        /// <summary>
        ///  API for Send Reset password Request for User Email {Get request from body}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("SendEmail-ResetUserPass")]
        public async Task<IActionResult> SendResetPassLink([FromBody] string EmailOrUserName)
        {
            var res = await userservice.SendResetPassEmail(EmailOrUserName);
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

        #region Active User
        /// <summary>
        ///  API for Register User Panel and make it Active {Get request from Route}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("Active-User/{ActivationCode}")]
        public async Task<IActionResult> ActiveUser([FromRoute] string ActivationCode)
        {
            var res = await userservice.ActiveUser(ActivationCode);
            switch (res)
            {
                case ResActiveUser.Success:
                    return JsonStatusResponse.Success("حساب کاربری شما با موفقیت فعال شد");
                case ResActiveUser.NotFoundOrActivated:
                    return JsonStatusResponse.Success("لینک فعالسازی نامعتبر است یا حساب کاربری قبلا فعال شده است");
                default:
                    return JsonStatusResponse.Success("عملیات با شکست مواجه شد");
            }
        }
        #endregion

        #region Singup
        /// <summary>
        ///  API for singup User{Get request from body}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("signUp-user")]
        public async Task<IActionResult> SingupUser([FromBody] ReqSingupUserDto userDto)
        {
            if (ModelState.IsValid)
            {
                var add = await userservice.SingUp(userDto);
                switch (add)
                {
                    case RessingupDto.success:
                        return JsonStatusResponse.Success("ثبت نام با موفقیت انجام شد");
                    case RessingupDto.Failed:
                        return JsonStatusResponse.Error("ثبت نام با خطا مواجه شد. مجدد ثبت نام کنید.");
                    case RessingupDto.Exist:
                        return JsonStatusResponse.Error("نام کاربری یا ایمیل قبلا ثبت نام شده است");
                    case RessingupDto.MobileExist:
                        return JsonStatusResponse.Error("شماره همراه قبلا ثبت نام شده است");
                    default:
                        HttpContext.Response.StatusCode = 400;
                        return JsonStatusResponse.Error("عملیات با خطا مواجه شد");


                }
            }

            return JsonStatusResponse.Error("فیلد‌های اجباری باید پر شوند");
        }
        #endregion

        #region Update profile
        /// <summary>
        ///  API for Update User Profile by user{Get request from form}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("Update-Profile")]
        public async Task<IActionResult> UpDate([FromForm] ReqUpdateUserDto userdto)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    var res = await userservice.UpDate(userdto, Convert.ToInt64(userId));
                    if (userdto.ProfilePhoto != null)
                    {
                        var resProfilePhoto = await userservice.ProfilePhotoUpload(userdto.ProfilePhoto, Convert.ToInt64(userId));
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
                            return JsonStatusResponse.Success("پروفایل کاربری با موفقیت بروزرسانی شد");
                        case ResUpdate.Error:
                            return JsonStatusResponse.Error("بروزرسانی پروفایل کاربری با خطا موجه شد. مجددا تلاش نمایید.");
                        case ResUpdate.NotFound:
                            return JsonStatusResponse.NotFound("ارتباط شما با سرور قطع شده است");
                        default:
                            return JsonStatusResponse.Error("عملیات با خطا مواجه شد");
                    }

                }
                return JsonStatusResponse.Error("اطلاعات ارسالی اشتباه است");
            }

            return JsonStatusResponse.Error("مجدد وارد پنل کاربری خود شوید.");
        }
        #endregion

        #region view profile
        /// <summary>
        ///  API for View User Panel by user{Get request from ...}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("View-Profile/{id}")]
        public async Task<IActionResult> ViewProfile([FromRoute]long id)
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userdto = await userservice.ViewProfile(Convert.ToInt64(userid));
            return JsonStatusResponse.Success(userdto, "موفق");


        }
        #endregion
    }
}

