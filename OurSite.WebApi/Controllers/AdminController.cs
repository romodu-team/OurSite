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

namespace OurSite.WebApi.Controllers
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

        #region Admin activities

        #region login
        /// <summary>
        ///  API for Login Admin into system {Get request from body}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login-Admin")]
        public async Task<IActionResult> LoginAdmin([FromBody] ReqLoginDto reqLogin)
        {
            if (ModelState.IsValid)
            {
                var admin = await adminService.Login(reqLogin);
                if (admin is null)
                    return JsonStatusResponse.NotFound("اطلاعات کاربری اشتباه است");
                var role = await roleService.GetAdminRole(admin.Id);

                var token = AuthenticationHelper.GenerateAdminToken(admin, role, 3);
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success(new { Token = token, Expire = 3, UserId = admin.Id, FirstName = admin.FirstName, LastName = admin.LastName }, "ورود با موفقیت انجام شد");
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

        #region Update admin profile
        /// <summary>
        ///  API for Update Admin Profile by system administrator {Get request from body}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Roles = "General Manager")]
        [HttpPut("Update-another-Admin-profile")]
        public async Task<IActionResult> UpdateAnotherAdmin([FromBody] ReqUpdateAdminDto req,long id)
        {
            if (ModelState.IsValid)
            {
                var res = await adminService.UpdateAdmin(req, id);
                if (req.ProfilePhoto != null)
                {
                    var resProfilePhoto = await adminService.ProfilePhotoUpload(req.ProfilePhoto, id);
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
                        return JsonStatusResponse.Success("با موفقیت ویرایش شد");
                    case ResUpdate.Error:
                        return JsonStatusResponse.Error("خطا در هنگام انجام عملیات");
                    default:
                        return JsonStatusResponse.Error("عملیات با شکست مواجه شد");
                }
            }
            return JsonStatusResponse.Error("اطلاعات وارد شده اشتباه است");
            
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

        #region Send Reset Password Emailupda
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
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("update-admin-profile")]
        public async Task<IActionResult> UpdateAdminBySelf([FromBody] ReqUpdateAdminDto req)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    var Adminid = User.FindFirst(ClaimTypes.NameIdentifier);
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
        #endregion

        #region Admin Management
        #region Change admin status
        /// <summary>
        ///  API for change status(is activity or not) Admin Panel {Get request from Route}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Roles = "General Manager,Admin")]
        [HttpGet("change-admin-status/{adminId}")]
        public async Task<IActionResult> ChangeAdminStatus([FromRoute] long adminId)
        {
            var res = await adminService.ChangeAdminStatus(adminId);
            if (res)
                return JsonStatusResponse.Success("وضعیت ادمین تغییر کرد");
            return JsonStatusResponse.Error("عملیات نا موفق بود");
        }
        #endregion


        #region Delete Admin Monharf
        /// <summary>
        ///  API for Delete Admin Panel by system administrator {Get request from Query}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[Authorize(Roles = "General Manager")]
        [HttpDelete("delete-admin")]
        public async Task<IActionResult> DeleteAdmin([FromQuery] long adminId)
        {
            var res = await adminService.DeleteAdmin(adminId);
            if (res)
                return JsonStatusResponse.Success("با موفقیت حذف شد");
            return JsonStatusResponse.Error("عملیات با شکست مواجه شد");
        }
        #endregion

        #region found admin by id
        /// <summary>
        ///  API for find Admin Profile by admin id{Get request from route}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("view-admin/{adminId}")]
        public async Task<IActionResult> GetAdmin([FromRoute] long adminId)
        {
            var res = await adminService.GetAdminById(adminId);
            if (res != null)
                return JsonStatusResponse.Success(res, "موفق");
            HttpContext.Response.StatusCode = 404;
            return JsonStatusResponse.NotFound("پیدا نشد");
        }
        #endregion

        #region List Admins
        /// <summary>
        ///  API for Get List of all admins{Get request from Query}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("Admin-list")]
        public async Task<IActionResult> GetAllAdmin([FromQuery] ReqFilterUserDto filter)
        {
            var list = await adminService.GetAllAdmin(filter);
            if (list.Admins.Any())
            {
                return JsonStatusResponse.Success(message: ("موفق"), ReturnData: list);

            }

            return JsonStatusResponse.NotFound(message: "آدمینی پیدا نشد");
        }
        #endregion


        #endregion



        #region Role Management

        #region Add new role
        /// <summary>
        ///  API for Add new role by system administrator {Get request from body}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Roles = "General Manager")] //add new role
        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromBody] RoleDto role)
        {
            var res = await roleService.AddRole(role);
            switch (res)
            {
                case ResAddRole.Success:
                    return JsonStatusResponse.Success("نقش با موفقیت اضافه شد");
                    break;
                case ResAddRole.Faild:
                    return JsonStatusResponse.Error("اضافه کردن نقش جدید با خطا مواجه شد. مجددا تلاش نمایید.");
                    break;

                case ResAddRole.InvalidInput:
                    return JsonStatusResponse.Error("فیلد‌های عنوان و تایتل نمی‌تواند خالی باشد.");
                    break;
                case ResAddRole.Exist:
                    return JsonStatusResponse.Error("این نقش قبلا اضافه شده است.");
                default:
                    return JsonStatusResponse.Error("عملیات با خطا مواجه شد.");
                    break;
            }
        }
        #endregion

        #region Delete a role
        /// <summary>
        ///  API for Delete role by system administrator {Get request from body}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Roles = "General Manager")] //remove role
        [HttpDelete("delete-role")]
        public async Task<IActionResult> RemoveRole([FromQuery] long RoleId)
        {
            var res = await roleService.RemoveRole(RoleId);
            if (res)
                return JsonStatusResponse.Success("نقش با موفقیت حذف شد");
            return JsonStatusResponse.Error("عملیات با شکست مواجه شد");
        }

        [Authorize(Roles = "General Manager")] //update role
        [HttpPut("update-role")]
        public async Task<IActionResult> UpdateRole(ReqUpdateRoleDto reqUpdate)
        {
            if (ModelState.IsValid)
            {
                var res = await roleService.UpdateRole(reqUpdate);
                switch (res)
                {
                    case ReqUpdateRoleDto.ResUpdateRole.Success:
                        return JsonStatusResponse.Success("نقش با موفقیت بروزرسانی شد");
                    case ReqUpdateRoleDto.ResUpdateRole.Error:
                        return JsonStatusResponse.Error("عملیات با شکست مواجه شد");
                    case ReqUpdateRoleDto.ResUpdateRole.NotFound:
                        return JsonStatusResponse.NotFound("نقش مورد نظر پیدا نشد");
                    case ReqUpdateRoleDto.ResUpdateRole.Exist:
                        return JsonStatusResponse.NotFound("نام نقش تکراری است");
                    default:
                        return JsonStatusResponse.Error("عملیات با شکست مواجه شد");
                }
            }
            return JsonStatusResponse.Error("فیلد های اجباری باید پر شوند");
        }
        #endregion

        #region Found role by role id
        /// <summary>
        ///  API for find role by role id {Get request from route}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Roles = "General Manager")] //Find role by id
        [HttpGet("view-Role/{RoleId}")]
        public async Task<IActionResult> GetRoleById([FromRoute] long RoleId)
        {
            var role = await roleService.GetRoleById(RoleId);
            if (role is null)
                return JsonStatusResponse.NotFound("نقش مورد نظر پیدا نشد");
            return JsonStatusResponse.Success(role, "موفق");
        }
        #endregion

        #region role list
        /// <summary>
        ///  API for Get list of all{Get request from Query}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Roles = "General Manager")] //find all role list
        [HttpGet("view-Roles")]
        public async Task<IActionResult> GetAllRoles([FromQuery]ReqFilterRolesDto filter)
        {
            var roles = await roleService.GetActiveRoles(filter);
            if (roles is null)
                return JsonStatusResponse.NotFound("هیچ نقشی پیدا نشد");
            return JsonStatusResponse.Success(roles, "موفق");
        }
        #endregion

        #endregion

        #region User management

        #region View user profile by admin
        /// <summary>
        ///  API for view user panel by admin {Get request from body}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("view-user/{id}")] //view user by admin
        public async Task<IActionResult> ViewUser([FromRoute]long userid)
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
        [Authorize(Roles = "General Manager,Admin")]
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
        public async Task<IActionResult> AddUser([FromBody]ReqAddUserAdminDto userDto)
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
        /// <returns></returns>
        [HttpPut("update-user-profile")]
        public async Task<IActionResult> UpdateUserByAdmin([FromForm]ReqUpdateUserDto userDto , long id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    var res = await userService.UpDate(userDto , id);
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
        public async Task<IActionResult> DeleteUser([FromQuery]long id)
        {
            var check = await userService.DeleteUser(id);
            if (check)
                return JsonStatusResponse.Success("کاربر با موفقیت حذف شد");
            return JsonStatusResponse.Error("عملیات ناموفق بود. ");

        }
        #endregion

        #endregion


    }


}