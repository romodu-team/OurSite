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
    public class AdminManagementContoroller : Controller
    {
        #region constructor
        private readonly IAdminService adminService;
        public AdminManagementContoroller(IAdminService adminService)
        {
            this.adminService = adminService;

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


        #region Change admin status
        /// <summary>
        ///  API for change status(is activity or not) Admin Panel {Get request from Route}
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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
                return JsonStatusResponse.Success(message: "موفق", ReturnData: list);

            }

            return JsonStatusResponse.NotFound(message: "آدمینی پیدا نشد");
        }
        #endregion

        #region Update admin profile
        /// <summary>
        ///  API for Update Admin Profile by system administrator {Get request from body}
        /// </summary>
        /// <remarks>The file size of the profile image must be less than 3 MB</remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("Update-another-Admin-profile")]
        public async Task<IActionResult> UpdateAnotherAdmin([FromBody] ReqUpdateAdminDto req, long id)
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
    }
}

