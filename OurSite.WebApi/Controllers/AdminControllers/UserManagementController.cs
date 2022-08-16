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
                return JsonStatusResponse.Success(message: "success", ReturnData: user);


            return JsonStatusResponse.Error(message: "کاربر پیدا نشد");
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
                return JsonStatusResponse.Success(message: "موفق", ReturnData: users);
            }
            return JsonStatusResponse.NotFound(message: "کاربری یافت نشد");

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
                return JsonStatusResponse.Success("وضعیت کاربر تغییر کرد");
            return JsonStatusResponse.Error("عملیات نا موفق بود");
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
                    return JsonStatusResponse.Success("کاربر با موفقیت ادد شد");
                    break;
                case ResadduserDto.Failed:
                    return JsonStatusResponse.Error("فیلد مربوطه نمیتواند خالی باشد");
                    break;
                case ResadduserDto.Exist:
                    return JsonStatusResponse.Error("نام کاربری وارد شده؛ وجود دارد.");
                default:
                    return JsonStatusResponse.Error("اضافه کردن کاربر با خطا مواجه شد. دوباره تلاش نمایید.");
                    break;
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
                            return JsonStatusResponse.Success("پنل کاربری شما با موفقیت بروزرسانی شد");
                        case ResUpdate.Error:
                            return JsonStatusResponse.Error("عملیات با خطا مواجه شد");
                        case ResUpdate.NotFound:
                            return JsonStatusResponse.NotFound("ارتباط شما با سرور قطع شده است");
                        default:
                            break;
                    }
                }
                return JsonStatusResponse.Error("اطلاعات ارسالی شما اشتباه است");
            }
            return JsonStatusResponse.Error("مجدد وارد پنل کاربری خود شوید.");

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
                return JsonStatusResponse.Success("کاربر با موفقیت حذف شد");
            return JsonStatusResponse.Error("عملیات ناموفق بود. ");

        }
        #endregion
    }
}

