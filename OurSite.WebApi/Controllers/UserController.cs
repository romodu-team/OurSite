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
        private IUserService userservice;

        public UserController(IUserService userService)
        {
            this.userservice = userService;

        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody]ReqLoginUserDto request)
        {
            
            var res = await userservice.LoginUser(request);
            switch (res)
            {
                case ResLoginDto.Success:
                 
                    var user =await userservice.GetUserByUserPass(request.UserName, request.Password);
                    var token = AuthenticationHelper.GenrateUserToken(user, 3);
                    HttpContext.Response.StatusCode = 200;
                    //  return new JsonResult(new {Token=token,Expire=3,UserId=user.Id,FirstName=user.FirstName,LastName=user.LastName});
                    return JsonStatusResponse.Success(new { Token = token, Expire = 3, UserId = user.Id, FirstName = user.FirstName, LastName = user.LastName },"ورود با موفقیت انجام شد");
                case ResLoginDto.IncorrectData:
                    //return new JsonResult(new { message = "نام کاربری یا رمز عبور اشتباه است" });
                    return JsonStatusResponse.NotFound("نام کاربری یا رمز عبور اشتباه است");
               
                case ResLoginDto.NotActived:
                    //return new JsonResult(new { message = "حساب کاربری شما فعال نیست" });
                    return JsonStatusResponse.Error("حساب کاربری شما فعال نیست");
                default:
                    HttpContext.Response.StatusCode = 400;
                    return JsonStatusResponse.Error("عملیات با خطا مواجه شد");

            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody]ReqForgotPassword request)
        {
            var res =await userservice.ForgotPassword(request);
            if (res)
                return JsonStatusResponse.Success("رمز عبور با موفقیت تغییر کرد");
            return JsonStatusResponse.Error("عملیات با شکست مواجه شد");
        }
        
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

        [HttpGet("Active-User/{ActivationCode}")]
        public async Task<IActionResult> ActiveUser([FromRoute]string ActivationCode)
        {
           var res= await userservice.ActiveUser(ActivationCode);
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
    }
}

