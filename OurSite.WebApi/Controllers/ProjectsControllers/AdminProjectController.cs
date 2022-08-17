using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OurSite.Core.DTOs.ProjectDtos;
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
        public AdminProjectController(IProject projectservice)
        {
            this.projectservice = projectservice;
        }


        #region Creat Project
        /// <summary>
        /// Api for Create project by admin for user {get request from body}
        /// </summary>
        /// <param name="prodto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("create-project")]
        public async Task<IActionResult> CreateProject([FromBody]CreateProjectDto prodto, long userId)
        {
            var res = await projectservice.CreateProject(prodto , userId);
            switch (res)
            {
                case ResProject.Success:
                    return JsonStatusResponse.Success("پروژه با موفقیت ایجاد شد.");
                case ResProject.Faild:
                    return JsonStatusResponse.Error("ایجاد پروژه با خطا مواجه شد.");
                case ResProject.InvalidInput:
                    return JsonStatusResponse.Error("فیلد‌های ثبت پروژه نمی‌تواند خالی باشد.");
                default:
                    return JsonStatusResponse.Error("ثبت پروژه با خطا مواجه شد. دقایقی دیگر مجدد تلاش نمایید.");
                    
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
        public async Task<IActionResult> EditProject([FromBody] EditProjectDto prodto)
        {
            var res = await projectservice.EditProject(prodto);
            switch (res)
            {
                case ResProject.Success:
                    return JsonStatusResponse.Success("The project has been updated successfully");
                case ResProject.Faild:
                    return JsonStatusResponse.Error("Project update failed. Try again ‌later.");
                case ResProject.NotFound:
                    return JsonStatusResponse.Error("Invalid input");
                default:
                    return JsonStatusResponse.Error("An error has occurred. Try again later.");
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
        public async Task<IActionResult> DeleteProject([FromQuery] long ProId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var remove = await projectservice.DeleteProject(ProId, true);
                switch (remove)
                {
                    case ResProject.Success:
                        return JsonStatusResponse.Success("The project has been deleted successfully");
                    case ResProject.Error:
                        return JsonStatusResponse.Error("Project delete failed. Try again later.");
                    case ResProject.NotFound:
                        return JsonStatusResponse.NotFound("Project not Found");
                    default:
                        return JsonStatusResponse.Error("An error has occurred. Try again later.");
                }
            }
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
        public async Task<IActionResult> GetProject([FromRoute]long ProjectId)
        {
            var res = await projectservice.GetProject(ProjectId);
            if (res is not null)
                return JsonStatusResponse.Success(ReturnData: res,message: "Project find successfully");

            return JsonStatusResponse.Error("Project Not found");
        }
        #endregion


        #region Upload contract File
        /// <summary>
        /// Api for Upload contract in projrct order {get request from body}
        /// </summary>
        /// <remarks>The size of the contract file must be less than 10 MB</remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Upload-Contract")]
        public async Task<IActionResult> UploadContract([FromBody]ReqUploadContractDto request)
        {
            var res = await projectservice.UploadContract(request);
            switch (res)
            {
                case resUploadContract.Success:
                    return JsonStatusResponse.Success("contract file uploaded successfully");
                case resUploadContract.projectNotFound:
                    return JsonStatusResponse.NotFound("project not found");
                case resUploadContract.FileNotFound:
                    return JsonStatusResponse.NotFound("contract file not found");
                case resUploadContract.Error:
                    return JsonStatusResponse.Error("server error");
                case resUploadContract.TooBig:
                    return JsonStatusResponse.Error("contract file is very big");
                case resUploadContract.FileExtentionError:
                    return JsonStatusResponse.Error("contract file extention invalid");
               
                default:
                    return JsonStatusResponse.Error("server error");
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
        public async Task<IActionResult> GetAllProject([FromQuery] ReqFilterProjectDto filter)
        {
            var projects = await projectservice.GetAllProject(filter);
            if (projects.Projects is not null && projects.Projects.Count>0)
            {
                return JsonStatusResponse.Success(message: "bia bekhoresh", ReturnData: projects);
            }
            return JsonStatusResponse.NotFound(message: "nist ke bekhorishi");
        }
        #endregion

    }
}

