using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto;
using OurSite.Core.DTOs;
using OurSite.Core.DTOs.AdminDtos;
using OurSite.Core.DTOs.UserDtos;
using OurSite.Core.Security;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Utilities;
using OurSite.DataLayer.Entities.Access;
using OurSite.DataLayer.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OurSite.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        #region constructor
        private IUserService userservice;
        private IRoleService Roleservice;
        private IGenericRepository<RefreshToken> _RefreshTokenRepository;

        public UserController(IRoleService Roleservice,IUserService userService, IGenericRepository<RefreshToken> refreshTokenRepository)
        {
            this.userservice = userService;
            this.Roleservice = Roleservice;
            _RefreshTokenRepository = refreshTokenRepository;
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
                AuthenticationHelper authenticationHelper = new AuthenticationHelper(Roleservice,_RefreshTokenRepository);
                var res = await userservice.LoginUser(request);
                switch (res)
                {
                    case ResLoginDto.Success:

                        var user = await userservice.GetUserByUserPass(request.UserName, request.Password);
                        var token = authenticationHelper.GenerateUserTokenAsync(user);
                        HttpContext.Response.StatusCode = 200;
                        return JsonStatusResponse.Success(new { Auth=token, Expire = 3, UserId = user.Id, FirstName = user.FirstName, LastName = user.LastName }, "login success");
                    case ResLoginDto.IncorrectData:
                        HttpContext.Response.StatusCode = 400;
                        return JsonStatusResponse.NotFound("Username ot password is wrong");

                    case ResLoginDto.NotActived:
                        HttpContext.Response.StatusCode = 401;
                        return JsonStatusResponse.Error("You not verified");

                    case ResLoginDto.Error:
                        HttpContext.Response.StatusCode = 400;
                        return JsonStatusResponse.Error("try agian later");
                    default:
                        HttpContext.Response.StatusCode = 500;
                        return JsonStatusResponse.UnhandledError();

                }
            }
            HttpContext.Response.StatusCode = 400;
            return JsonStatusResponse.InvalidInput();
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
                {
                    HttpContext.Response.StatusCode = 200;
                    return JsonStatusResponse.Success("Your password has been changed");
                }
                HttpContext.Response.StatusCode = 500;
                return JsonStatusResponse.Error("try agian later");
            }
            HttpContext.Response.StatusCode = 400;
            return JsonStatusResponse.InvalidInput();
        }
        #endregion

        #region Send Reset Password Email
        /// <summary>
        ///  API for Send Reset password Request for User Email {Get request from body}
        /// </summary>
        /// <param name="EmailOrUserName"></param>
        /// <returns></returns>
        [HttpPost("SendEmail-ResetUserPass")]
        public async Task<IActionResult> SendResetPassLink([FromBody] string EmailOrUserName)
        {
            var res = await userservice.SendResetPassEmail(EmailOrUserName);
            switch (res)
            {
                case ResLoginDto.Success:
                    HttpContext.Response.StatusCode = 200;
                    return JsonStatusResponse.Success("Reset password link send to your email.");
                case ResLoginDto.IncorrectData:
                    HttpContext.Response.StatusCode = 404;
                    return JsonStatusResponse.NotFound("account not found.");

                default:
                    HttpContext.Response.StatusCode = 400;
                    return JsonStatusResponse.UnhandledError();
            }

        }
        #endregion

        #region Active User
        /// <summary>
        ///  API for Register User Panel and make it Active {Get request from Route}
        /// </summary>
        /// <param name="ActivationCode"></param>
        /// <returns></returns>
        [HttpGet("Active-User/{ActivationCode}")]
        public async Task<IActionResult> ActiveUser([FromRoute] string ActivationCode)
        {
            var res = await userservice.ActiveUser(ActivationCode);
            switch (res)
            {
                case ResActiveUser.Success:
                    HttpContext.Response.StatusCode = 200;
                    return JsonStatusResponse.Success("You account has been active");
                case ResActiveUser.NotFoundOrActivated:
                    HttpContext.Response.StatusCode = 400;
                    return JsonStatusResponse.NotFound("Your account actived or active link invalid");
                default:
                    HttpContext.Response.StatusCode = 500;
                    return JsonStatusResponse.UnhandledError();
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
                        HttpContext.Response.StatusCode = 200;
                        return JsonStatusResponse.Success("You singup successfully");
                    case RessingupDto.Failed:
                        HttpContext.Response.StatusCode = 500;
                        return JsonStatusResponse.Error("Your singup failed");
                    case RessingupDto.Exist:
                        HttpContext.Response.StatusCode = 409;
                        return JsonStatusResponse.Error("You singup before");
                    case RessingupDto.MobileExist:
                        HttpContext.Response.StatusCode = 409;
                        return JsonStatusResponse.Error("this phone number is exist");
                    default:
                        HttpContext.Response.StatusCode = 500;
                        return JsonStatusResponse.UnhandledError();


                }
            }
            HttpContext.Response.StatusCode = 500;
            return JsonStatusResponse.InvalidInput();
        }
        #endregion

        #region Update profile
        /// <summary>
        ///  API for Update User Profile by user{Get request from form}
        /// </summary>
        /// <param name="request"></param>
        /// <remarks>The file size of the profile image must be less than 3 MB</remarks>
        /// <returns></returns>
        [HttpPut("Update-Profile")]
        [Authorize]
        public async Task<IActionResult> UpDate([FromForm] ReqUpdateUserDto userdto)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    var userId = User.FindFirst(ClaimTypes.Sid).Value;
                    var res = await userservice.UpDate(userdto, Convert.ToInt64(userId));
                    if (userdto.ProfilePhoto != null)
                    {
                        var resProfilePhoto = await userservice.ProfilePhotoUpload(userdto.ProfilePhoto, Convert.ToInt64(userId));
                        switch (resProfilePhoto)
                        {
                            case resFileUploader.Failure:
                                HttpContext.Response.StatusCode = 500;
                                return JsonStatusResponse.Error("upload profile photot faild");

                            case resFileUploader.ToBig:
                                HttpContext.Response.StatusCode = 400;
                                return JsonStatusResponse.Error("the upload file is too big");

                            case resFileUploader.NoContent:
                                HttpContext.Response.StatusCode = 204;
                                return JsonStatusResponse.InvalidInput();
                            case resFileUploader.InvalidExtention:
                                HttpContext.Response.StatusCode = 400;
                                return JsonStatusResponse.Error("photo format is not true");
                            default:
                                HttpContext.Response.StatusCode = 500;
                                return JsonStatusResponse.UnhandledError();
                                break;
                        }
                    }
                    switch (res)
                    {
                        case ResUpdate.Success:
                            HttpContext.Response.StatusCode = 200;
                            return JsonStatusResponse.UnhandledError();
                            return JsonStatusResponse.Success("photo has been upload successfully");
                        case ResUpdate.Error:
                            HttpContext.Response.StatusCode = 200;
                            return JsonStatusResponse.Error("photo has been update successfully");
                        case ResUpdate.NotFound:
                            HttpContext.Response.StatusCode = 404;
                            return JsonStatusResponse.NotFound("server error");
                        default:
                            HttpContext.Response.StatusCode = 500;
                            return JsonStatusResponse.UnhandledError();
                    }

                }
                HttpContext.Response.StatusCode = 400;
                return JsonStatusResponse.InvalidInput();
            }
            HttpContext.Response.StatusCode = 401;
            return JsonStatusResponse.Error("Login agian");
        }
        #endregion

        #region view profile
        /// <summary>
        ///  API for View User Panel by user{Get request from ...}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("View-Profile/{id}")]
        [Authorize]
        public async Task<IActionResult> ViewProfile([FromRoute]long id)
        {
            var userid = User.FindFirst(ClaimTypes.Sid).Value;
            var userdto = await userservice.ViewProfile(Convert.ToInt64(userid));
            HttpContext.Response.StatusCode = 200;
            return JsonStatusResponse.Success(userdto, "success");


        }
        #endregion
    }
}

