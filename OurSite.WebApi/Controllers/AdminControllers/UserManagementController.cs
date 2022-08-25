using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OurSite.Core.DTOs.AdminDtos;
using OurSite.Core.DTOs.UserDtos;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Utilities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OurSite.WebApi.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    public class UserManagementController : Controller
    {
        private readonly IAdminService adminService;
        private readonly IUserService userService;
        public UserManagementController(IAdminService adminService, IUserService userService)
        {
            this.adminService = adminService;
            this.userService = userService;

        }
        #region View user profile by admin
        /// <summary>
        ///  API for view user panel by admin {Get request from body}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("view-user/{userid}")] //view user by admin
        public async Task<IActionResult> ViewUser([FromRoute] long userid)
        {
            var user = await userService.ViewUser(userid);
            if (user is not null)
            {
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success(message: "success", ReturnData: user);
            }
            HttpContext.Response.StatusCode = 404;
            return JsonStatusResponse.Error("User not found");
        }
        #endregion

        #region User list
        /// <summary>
        ///  API for Get list of all user by admin {Get request from Query}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("view-all-users")] //Get user list
        public async Task<IActionResult> GetAllUser([FromQuery] ReqFilterUserDto filter)
        {
            var users = await userService.GetAlluser(filter);
            if (users.Users.Any())
            {
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success(message: "success", ReturnData: users);
            }
            HttpContext.Response.StatusCode = 404;
            return JsonStatusResponse.NotFound("Users not found");

        }
        #endregion

        #region Change user status
        /// <summary>
        ///  API for change user status(is activity or not) by admin {Get request from route}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("change-user-status/{userId}")]
        public async Task<IActionResult> ChangeUserStatus([FromRoute] long userId)
        {
            var res = await userService.ChangeUserStatus(userId);
            if (res)
            {
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success("user status has been changed");
            }
            HttpContext.Response.StatusCode = 500;
            return JsonStatusResponse.Error("user status has not been changed");
        }
        #endregion

        #region Add user by admin
        /// <summary>
        ///  API for add new user by admin {Get request from body}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[Authorize(Roles = "General Manager,Admin")]
        [HttpPost("add-user")]
        public async Task<IActionResult> AddUser([FromBody] ReqAddUserAdminDto userDto)
        {
            var add = await userService.AddUser(userDto);
            switch (add)
            {
                case ResadduserDto.success:
                    HttpContext.Response.StatusCode = 200;
                    return JsonStatusResponse.Success("user add successfully");
                case ResadduserDto.Failed:
                    HttpContext.Response.StatusCode = 400;
                    return JsonStatusResponse.InvalidInput();
                case ResadduserDto.Exist:
                    HttpContext.Response.StatusCode = 409;
                    return JsonStatusResponse.Error("username exist");
                default:
                    HttpContext.Response.StatusCode = 500;
                    return JsonStatusResponse.UnhandledError();
            }
        }

        #endregion

        #region update user's profile by admin
        /// <summary>
        ///  API for update user by admin {Get request from form}
        /// </summary>
        /// <param name="request"></param>
        /// <remarks>The file size of the profile image must be less than 3 MB</remarks>
        /// <returns></returns>
        [HttpPut("update-user-profile")]
        public async Task<IActionResult> UpdateUserByAdmin([FromForm] ReqUpdateUserDto userDto, long id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    var res = await userService.UpDate(userDto, id);
                    if (userDto.ProfilePhoto != null)
                    {
                        var resProfilePhoto = await userService.ProfilePhotoUpload(userDto.ProfilePhoto, id);
                        switch (resProfilePhoto)
                        {
                            case resFileUploader.Failure:
                                HttpContext.Response.StatusCode = 500;
                                return JsonStatusResponse.Error("update profile failed");

                            case resFileUploader.ToBig:
                                HttpContext.Response.StatusCode = 413;
                                return JsonStatusResponse.Error("the file is larg");
                            case resFileUploader.NoContent:
                                HttpContext.Response.StatusCode = 204;
                                return JsonStatusResponse.Error("no content in filed photo ");
                            case resFileUploader.InvalidExtention:
                                HttpContext.Response.StatusCode = 400;
                                return JsonStatusResponse.Error("photo format is inccurent");
                            default:
                                HttpContext.Response.StatusCode = 500;
                                return JsonStatusResponse.UnhandledError();
                        }
                    }
                    switch (res)
                    {

                        case ResUpdate.Success:
                            HttpContext.Response.StatusCode = 200;
                            return JsonStatusResponse.Success("profile has been updated successfully");
                        case ResUpdate.Error:
                            HttpContext.Response.StatusCode = 500;
                            return JsonStatusResponse.Error("profile has not been changed");
                        case ResUpdate.NotFound:
                            HttpContext.Response.StatusCode = 403;
                            return JsonStatusResponse.Error("login agian. your token is invalid");
                        default:
                            HttpContext.Response.StatusCode = 500;
                            return JsonStatusResponse.UnhandledError();
                    }
                }
                HttpContext.Response.StatusCode = 400;
                return JsonStatusResponse.InvalidInput();
            }
            HttpContext.Response.StatusCode = 403;
            return JsonStatusResponse.Error("login agian. your token is invalid");

        }

        #endregion

        #region Delete user
        /// <summary>
        ///  API for delete user by admin {Get request from route}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete("delete-user")]
        public async Task<IActionResult> DeleteUser([FromQuery] long id)
        {
            var check = await userService.DeleteUser(id);
            if (check)
            {
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success("user has been deleted successfully");
            }
            HttpContext.Response.StatusCode = 500;
            return JsonStatusResponse.Error("user has not been deleted");

        }
        #endregion
    }
}

