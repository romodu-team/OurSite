using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OurSite.Core.DTOs.ProjectDtos;
using OurSite.Core.Services.Interfaces.Projecta;
using OurSite.Core.Utilities;
using static OurSite.Core.DTOs.ProjectDtos.CreatProjectDto;

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
        
        #region Creat Project
        /// <summary>
        /// Api for Creat project by user
        /// </summary>
        /// <param name="prodto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("creat-project")]
        public async Task<IActionResult> CreateProject([FromBody]CreatProjectDto prodto, long userId)
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


        #region Delete project
        /// <summary>
        /// Api for Delete project by user {Get request from body}
        /// </summary>
        /// <param name="ReqDeleteProject"></param>
        /// <returns></returns>
        [HttpDelete("user-delete-project")]
        public async Task<IActionResult> DeleteProject([FromBody] DeleteProjectDto ReqDeleteProject)
        {
            var remove = await projectservice.DeleteProject(ReqDeleteProject);
            switch (remove)
            {
                case ResProject.Success:
                    return JsonStatusResponse.Success("The project has been deleted successfully");
                case ResProject.Error:
                    return JsonStatusResponse.Error("Project delete failed. Try again later.");
                case ResProject.SitutionError:
                    return JsonStatusResponse.Error("Can't delete project at this time.");
                case ResProject.NotFound:
                    return JsonStatusResponse.NotFound("Project not Found");
                default:
                    return JsonStatusResponse.Error("An error has occurred. Try again later.");
            }
        }
        #endregion


    }
}

