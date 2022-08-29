using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OurSite.Core.DTOs.ProjectDtos;
using OurSite.Core.Services.Interfaces.Projecta;
using OurSite.Core.Utilities;
using static OurSite.Core.DTOs.ProjectDtos.CreateProjectDto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OurSite.WebApi.Controllers.ProjectsControllers
{
    [Route("api/[controller]")]
    public class UserProjectController : Controller
    {
        private IProject projectservice;
        public UserProjectController(IProject projectservice)
        {
            this.projectservice = projectservice;
        }
        
        #region Create Project
        /// <summary>
        /// Api for Create project by user
        /// </summary>
        /// <param name="prodto"></param>
        /// <returns></returns>
        [HttpPost("create-project-by-User")]
        public async Task<IActionResult> CreateProject([FromBody]CreateProjectDto prodto)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirst(ClaimTypes.Sid).Value;
                var res = await projectservice.CreateProject(prodto, Convert.ToInt64(userId));
                switch (res)
                {
                    case ResProject.Success:
                        HttpContext.Response.StatusCode = 200;
                        return JsonStatusResponse.Success(message: "The project has been create successfully", ReturnData: prodto);
                    case ResProject.Faild:
                        HttpContext.Response.StatusCode = 500;
                        return JsonStatusResponse.Error("Project Create failed. Try again later.");
                    case ResProject.InvalidInput:
                        HttpContext.Response.StatusCode = 400;
                        return JsonStatusResponse.Error("Invalid input erorr");
                    default:
                        HttpContext.Response.StatusCode = 500;
                        return JsonStatusResponse.UnhandledError();

                }
            }
            HttpContext.Response.StatusCode = 403;
            return JsonStatusResponse.Error("U must login");
            
        }
        #endregion


        #region Delete project
        /// <summary>
        /// Api for Delete project by user {Get request from body}
        /// </summary>
        /// <param name="proId"></param>
        /// <returns></returns>
        [HttpDelete("user-delete-project")]
        public async Task<IActionResult> DeleteProject([FromQuery] long proId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var remove = await projectservice.DeleteProject(proId, false);
                switch (remove)
                {
                    case ResProject.Success:
                        HttpContext.Response.StatusCode = 200;
                        return JsonStatusResponse.Success(message: "The project has been deleted successfully" , ReturnData: proId);
                    case ResProject.Error:
                        HttpContext.Response.StatusCode = 500;
                        return JsonStatusResponse.Error("Project delete failed. Try again later.");
                    case ResProject.SitutionError:
                        HttpContext.Response.StatusCode = 400;
                        return JsonStatusResponse.Error("Can't delete project at this time.");
                    case ResProject.NotFound:
                        HttpContext.Response.StatusCode = 404;
                        return JsonStatusResponse.NotFound("Project not Found");
                    default:
                        HttpContext.Response.StatusCode = 200;
                        return JsonStatusResponse.UnhandledError();
                }
            }
            HttpContext.Response.StatusCode = 403;
            return JsonStatusResponse.Error("u didnt login. please login first");

        }
        #endregion

        #region View project
        /// <summary>
        /// Api for get one project by user {Get request from route}
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        [HttpGet("view-project-by-user/{ProjectId}")]
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

        #region Get project by user
        /// <summary>
        /// Api for get projects list by user {get request from query}
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("project-list")]
        public async Task<IActionResult> GetAllProject([FromQuery] ReqFilterProjectDto filter)
        {
            var projects = await projectservice.GetAllProject(filter);
            if (projects.Projects is not null && projects.Projects.Count>0)
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

