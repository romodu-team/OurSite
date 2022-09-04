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
                switch (res.RessingupDto)
                {
                    case RessingupDto.success:
                        HttpContext.Response.StatusCode = 201;
                        return JsonStatusResponse.Success(new {AdminId=res.AdminId,AdminUUID=res.AdminUUID },"Admin has been registered");
                    case RessingupDto.Exist:
                        HttpContext.Response.StatusCode = 409;
                        return JsonStatusResponse.Error("Username or Email exist");
                    default:
                        HttpContext.Response.StatusCode = 500;
                        return JsonStatusResponse.UnhandledError();
                }
            }
            HttpContext.Response.StatusCode = 400;
            return JsonStatusResponse.InvalidInput();

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
            {
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success("Admin status has been changed sucessfully");
            }
            HttpContext.Response.StatusCode = 500;
            return JsonStatusResponse.Error("Admin status has not been changed");
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
            {
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success("Admin deleted");
            }
            HttpContext.Response.StatusCode = 500;
            return JsonStatusResponse.Error("delete admin failed");
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
            {
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success(res, "success");
            }

            HttpContext.Response.StatusCode = 404;
            return JsonStatusResponse.NotFound("not found");
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
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success(message: "success", ReturnData: list);

            }
            HttpContext.Response.StatusCode = 404;
            return JsonStatusResponse.NotFound(message: "not found");
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
        public async Task<IActionResult> UpdateAnotherAdmin([FromForm] ReqUpdateAdminDto req, long id)
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
                            HttpContext.Response.StatusCode = 500;
                            return JsonStatusResponse.Error("photo not uploaded");

                        case resFileUploader.ToBig:
                            HttpContext.Response.StatusCode = 413;
                            return JsonStatusResponse.Error("file size is large");
                        case resFileUploader.NoContent:
                            HttpContext.Response.StatusCode = 204;
                            return JsonStatusResponse.Error("You didnt upload any file");
                        case resFileUploader.InvalidExtention:
                            HttpContext.Response.StatusCode = 400;
                            return JsonStatusResponse.Error("file format is incurrent");
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
                        return JsonStatusResponse.Success("profile updated");
                    case ResUpdate.Error:
                        HttpContext.Response.StatusCode = 500;
                        return JsonStatusResponse.Error("update profile failed");
                    default:
                        HttpContext.Response.StatusCode = 500;
                        return JsonStatusResponse.UnhandledError();
                }
            }
            HttpContext.Response.StatusCode = 500;
            return JsonStatusResponse.InvalidInput();

        }
        #endregion
    }
}

