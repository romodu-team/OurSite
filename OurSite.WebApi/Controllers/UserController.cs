using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OurSite.Core.DTOs;
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
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody]ReqLoginDto request)
        {
            
            var res = await userservice.LoginUser(request);
            switch (res)
            {
                case ResLoginDto.Success:
                 
                    var user =await userservice.GetUserByUserPass(request.UserName, request.Password);
                    var token = AuthenticationHelper.GenrateUserToken(user, 3);
                    HttpContext.Response.StatusCode = 200;
                    return JsonStatusResponse.Success(new { Token = token, Expire = 3, UserId = user.Id, FirstName = user.FirstName, LastName = user.LastName },"ورود با موفقیت انجام شد");
                case ResLoginDto.IncorrectData:

                    return JsonStatusResponse.NotFound("نام کاربری یا رمز عبور اشتباه است");
               
                case ResLoginDto.NotActived:
                    return JsonStatusResponse.Error("حساب کاربری شما فعال نیست");
                default:
                    HttpContext.Response.StatusCode = 400;
                    return JsonStatusResponse.Error("عملیات با خطا مواجه شد");

            }
        }
        #endregion

        #region Forget Password
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody]ReqForgotPassword request)
        {
            var res =await userservice.ForgotPassword(request);
            if (res)
                return JsonStatusResponse.Success("رمز عبور با موفقیت تغییر کرد");
            return JsonStatusResponse.Error("عملیات با شکست مواجه شد");
        }
        #endregion

        #region Reset Password
        [HttpPost("SendEmail-ResetPass")]
        public async Task<IActionResult> SendResetPassLink([FromBody]string EmailOrUserName)
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
        [HttpGet("Active-User/{ActivationCode}")]
        public async Task<IActionResult> ActiveUser([FromRoute] string ActivationCode)
        {
            var res = await userservice.ActiveUser(ActivationCode);
            switch (res)
            {
                case ResActiveUser.Success:
                    return JsonStatusResponse.Success("حساب کاربری شما با موفقیت فعال شد");
                case ResActiveUser.Failed:
                    return JsonStatusResponse.Success("لینک فعالسازی نامعتبر است یا حساب کاربری قبلا فعال شده است");
                default:
                    return JsonStatusResponse.Success("عملیات با شکست مواجه شد");
            }
        }
        #endregion

        #region Singup 
        public async Task<IActionResult> SingupUser(ReqSingupUserDto userDto)
        {
            var add = await userservice.SingUp(userDto);
            switch (add)
            {
                case singup.success:
                    return JsonStatusResponse.Success("ثبت نام با موفقیت انجام شد");
                case singup.Failed:
                    return JsonStatusResponse.Error("ثبت نام با خطا مواجه شد. مجدد ثبت نام کنید.");
                case singup.Exist:
                    return JsonStatusResponse.Error("نام کاربری یا ایمیل قبلا ثبت نام شده است");
                default:
                    HttpContext.Response.StatusCode = 400;
                    return JsonStatusResponse.Error("عملیات با خطا مواجه شد");


            }
        }
        #endregion

        #region Update profile
        public async Task<IActionResult> UpDate(ReqUpdateUserDto userdto)
        {
            if(ModelState.IsValid)
            {
                var res = await userservice.UpDate(userdto);
                if (res)
                {
                    return JsonStatusResponse.Success("پروفایل کاربری با موفقیت بروزرسانی شد");

                }
                else
                {
                    return JsonStatusResponse.Error("بروزرسانی پروفایل کاربری با خطا موجه شد. مجددا تلاش نمایید.");
                }
            }
            return JsonStatusResponse.Error("اطلاعات ارسالی اشتباه است");

        }
        #endregion
    }
}

