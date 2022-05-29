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
        private IPasswordHelper passwordHelper;
        public UserController(IUserService userService, IPasswordHelper passwordHelper)
        {
            this.userservice = userService;
            this.passwordHelper = passwordHelper;
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody]ReqLoginUserDto request)
        {
            request.Password = passwordHelper.EncodePasswordMd5(request.Password);
            var res = await userservice.LoginUser(request);
            switch (res)
            {
                case ResLoginDto.Success:
                 
                    var user =await userservice.GetUserByUserPass(request.UserName, request.Password);
                    var token = AuthenticationHelper.GenrateUserToken(user, 3);
                    HttpContext.Response.StatusCode = 200;
                    return new JsonResult(new {Token=token,Expire=3,UserId=user.Id,FirstName=user.FirstName,LastName=user.LastName});
                  
                case ResLoginDto.IncorrectData:
                    return new JsonResult(new { message = "نام کاربری یا رمز عبور اشتباه است" });
               
                case ResLoginDto.NotActived:
                    return new JsonResult(new { message = "حساب کاربری شما فعال نیست" });
                default:
                    HttpContext.Response.StatusCode = 400;
                    return new JsonResult(new { message = "عملیات با خطا مواجه شد" });
            }
        }
        
    }
}

