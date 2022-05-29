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
            this.userservice = userservice;
            this.passwordHelper = passwordHelper;
        }

        public async Task<IActionResult> LoginUser(ReqLoginUserDto request)
        {
            request.Password = passwordHelper.EncodePasswordMd5(request.Password);
            var res = await userservice.LoginUser(request);
            switch (res)
            {
                case ResLoginDto.Success:
                    //jwt token
                    var user =await userservice.GetUserByUserPass(request.UserName, request.Password);
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("sajjadhaniehfaezeherfanmobinsinamehdi"));
                    var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var tokenOption = new JwtSecurityToken(
                        issuer: "https://localhost:7181",
                        claims: new List<Claim>()
                        {
                                new Claim(ClaimTypes.Name, String.Concat(user.FirstName,user.LastName)),
                                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
                        },
                        expires: DateTime.Now.AddDays(5),
                        signingCredentials: signInCredentials
                    );
                    var token = new JwtSecurityTokenHandler().WriteToken(tokenOption);
                    return JsonStatusResponse.Success();
                    break;
                case ResLoginDto.IncorrectData:
                    return JsonStatusResponse.Error(new { message = "Incorrect Data" });
                    break;
                case ResLoginDto.NotActived:
                    return JsonStatusResponse.Error(new { message = "Not Actived" });
                    break;
                
            }
        }
        
    }
}

