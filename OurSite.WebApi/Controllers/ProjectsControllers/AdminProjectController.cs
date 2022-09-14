using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OurSite.Core.DTOs.MailDtos;
using OurSite.Core.DTOs.NotificationDtos;
using OurSite.Core.DTOs.ProjectDtos;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Services.Interfaces.Mail;
using OurSite.Core.Services.Interfaces.Projecta;
using OurSite.Core.Utilities;
using static OurSite.Core.DTOs.ProjectDtos.CreateProjectDto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OurSite.WebApi.Controllers.ProjectsControllers
{
    [Route("api/[controller]")]
    public class AdminProjectController : Controller
    {
        private IProject projectservice;
        private INotificationService _notificationService;
        private IUserService _UserService;
        private IMailService _mailService;
        public AdminProjectController(IMailService mailService, IUserService UserService, IProject projectservice, INotificationService notificationService)
        {
            _mailService = mailService;
            this.projectservice = projectservice;
            _notificationService = notificationService;
            _UserService = UserService;
        }


        #region Creat Project
        /// <summary>
        /// Api for Create project by admin for user {get request from body}
        /// </summary>
        /// <param name="prodto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("create-project")]
        [Authorize(Policy = StaticPermissions.PermissionAdminCreateProject)]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto prodto, long userId)
        {
            var res = await projectservice.CreateProject(prodto, userId);
            switch (res)
            {
                case ResProject.Success:
                    var user = await _UserService.GetUserById(userId);
                    await _notificationService.CreateNotification(new ReqCreateNotificationDto { AccountUUID = user.UUID, Message = $"پروژه جدید با نام {prodto.Name}برای شما ایجاد شد" });
                    if (user.Email != null)
                        await _mailService.SendEmailAsync(new MailRequestDto { ToEmail = user.Email, Subject = $"پروژه جدید با نام {prodto.Name}برای شما ایجاد شد", Body = "جزییات پروژه" });
                    HttpContext.Response.StatusCode = 200;
                    return JsonStatusResponse.Success(message: "Project creat sucessfully" , ReturnData: prodto);
                case ResProject.Faild:
                    HttpContext.Response.StatusCode = 500;
                    return JsonStatusResponse.Error("Project creat Faild");
                case ResProject.InvalidInput:
                    HttpContext.Response.StatusCode = 400;
                    return JsonStatusResponse.Error("Fileds cant be empty");
                default:
                    HttpContext.Response.StatusCode = 500;
                    return JsonStatusResponse.UnhandledError();
                    
            }
        }
        #endregion

        #region Edit Project
        /// <summary>
        /// Api for edit project details by admin {get request from body}
        /// </summary>
        /// <param name="prodto"></param>
        /// <returns></returns>
        [HttpPut("edit-Project")]
        [Authorize(Policy = StaticPermissions.PermissionAdminEditProject)]

        public async Task<IActionResult> EditProject([FromBody] EditProjectDto prodto)
        {
            var res = await projectservice.EditProject(prodto);
            switch (res)
            {
                case ResProject.Success:
                    HttpContext.Response.StatusCode = 200;
                    return JsonStatusResponse.Success(message:"The project has been updated successfully" , ReturnData: prodto);
                case ResProject.Faild:
                    HttpContext.Response.StatusCode = 500;
                    return JsonStatusResponse.Error("Project update failed. Try again ‌later.");
                case ResProject.NotFound:
                    HttpContext.Response.StatusCode = 404;
                    return JsonStatusResponse.Error("Invalid input");
                default:
                    HttpContext.Response.StatusCode = 500;
                    return JsonStatusResponse.UnhandledError();
            }
        }
        #endregion

        #region Delete project
        /// <summary>
        /// Api for Delete project by admin{Get request from body}
        /// </summary>
        /// <param name="ProId"></param>
        /// <returns></returns>
        [HttpDelete("admin-delete-project")]
        [Authorize(Policy = StaticPermissions.PermissionAdminDeleteProject)]

        public async Task<IActionResult> DeleteProject([FromQuery] long ProId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var remove = await projectservice.DeleteProject(ProId, true);
                switch (remove)
                {
                    case ResProject.Success:
                        HttpContext.Response.StatusCode = 200;
                        return JsonStatusResponse.Success(message: "The project has been deleted successfully" , ReturnData: ProId);
                    case ResProject.Error:
                        HttpContext.Response.StatusCode = 500;
                        return JsonStatusResponse.Error("Project delete failed. Try again later.");
                    case ResProject.NotFound:
                        HttpContext.Response.StatusCode = 404;
                        return JsonStatusResponse.NotFound("Project not Found");
                    default:
                        HttpContext.Response.StatusCode = 500;
                        return JsonStatusResponse.UnhandledError();
                }
            }
            HttpContext.Response.StatusCode = 403;
            return JsonStatusResponse.Error("u didnt login. please login first");

        }
        #endregion

        #region View project
        /// <summary>
        /// Api for get one project by admin {Get request from route}
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        [HttpGet("view-project-by-admin/{ProjectId}")]
        [Authorize(Policy = StaticPermissions.PermissionAdminViewProject)]

        public async Task<IActionResult> GetProject([FromRoute] long ProjectId)
        {
            var res = await projectservice.GetProject(ProjectId);
            if (res is not null)
            {
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success(ReturnData: res, message: "Project find successfully");
            }
            HttpContext.Response.StatusCode = 404;
            return JsonStatusResponse.Error("Project Not found");
        }
        #endregion

        #region Upload contract File
        /// <summary>
        /// Api for Upload contract in projrct order {get request from form}
        /// </summary>
        /// <remarks>The size of the contract file must be less than 10 MB</remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Upload-Contract")]
        [Authorize(Policy = StaticPermissions.PermissionAdminUploadProjectContracts)]

        public async Task<IActionResult> UploadContract([FromForm] ReqUploadContractDto request)
        {
            var res = await projectservice.UploadContract(request);
            switch (res)
            {
                case resUploadContract.Success:
                    HttpContext.Response.StatusCode = 200;
                    var contract = await projectservice.ReturnContract(request.ProjectId);
                    return JsonStatusResponse.Success(message: "contract file uploaded successfully" , ReturnData: contract);
                case resUploadContract.projectNotFound:
                    HttpContext.Response.StatusCode = 404;
                    return JsonStatusResponse.NotFound("project not found");
                case resUploadContract.FileNotFound:
                    HttpContext.Response.StatusCode = 404;
                    return JsonStatusResponse.NotFound("contract file not found");
                case resUploadContract.Error:
                    HttpContext.Response.StatusCode = 500;
                    return JsonStatusResponse.Error("server error");
                case resUploadContract.TooBig:
                    HttpContext.Response.StatusCode = 413;
                    return JsonStatusResponse.Error("contract file is very big");
                case resUploadContract.FileExtentionError:
                    HttpContext.Response.StatusCode = 409;
                    return JsonStatusResponse.Error("contract file extention invalid");

                default:
                    HttpContext.Response.StatusCode = 500;
                    return JsonStatusResponse.UnhandledError();
            }
        }

        #endregion

        #region Project list
        /// <summary>
        /// Api for get projects list{get request from query}
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("project-list")]
        [Authorize(Policy = StaticPermissions.PermissionAdminViewAllProject)]
        public async Task<IActionResult> GetAllProject([FromQuery] ReqFilterProjectDto filter)
        {
            var projects = await projectservice.GetAllProject(filter);
            if (projects.Projects is not null && projects.Projects.Count > 0)
            {
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success(message: "bia bekhoresh", ReturnData: projects);
            }
            HttpContext.Response.StatusCode = 404;
            return JsonStatusResponse.NotFound(message: "nist ke bekhorishi");
        }
        #endregion

    }
}

