using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OurSite.Core.DTOs.ProjectDtos;
using OurSite.Core.Services.Interfaces.Projects;
using OurSite.Core.Utilities;
using static OurSite.Core.DTOs.ProjectDtos.CreatProjectDto;

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
        /// Creat project by admin for user {get request from body}
        /// </summary>
        /// <param name="prodto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("creat-project")]
        public async Task<IActionResult> CreatProject([FromBody]CreatProjectDto prodto, long userId)
        {
            var res = await projectservice.CreatProject(prodto , userId);
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
        /// edit project details by admin {get request from body}
        /// </summary>
        /// <param name="prodto"></param>
        /// <returns></returns>
        [HttpPut("edite-Project")]
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
                    break;
            }
        }
        #endregion
    }
}

