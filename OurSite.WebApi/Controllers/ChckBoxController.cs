using Microsoft.AspNetCore.Mvc;
using OurSite.Core.DTOs.CheckBoxDtos;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Utilities;

namespace OurSite.WebApi.Controllers;
 [Route("api/[controller]")]
 [ApiController]
public class ChckBoxController:Controller
{
    #region Constructor
        private ICheckBoxService _CheckBoxService;
        public ChckBoxController( ICheckBoxService CheckBoxService)
        {
            _CheckBoxService = CheckBoxService;
        }

        #endregion
        /// <summary>
        /// create a CheckBox for project or cosultion form
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("create-CheckBox")]
        public async Task<IActionResult> CreateCategory(ReqCreateCheckBoxDto request)
        {
            var res = await _CheckBoxService.CreateCheckBox(request.Title,request.IconName,request.Description,request.SiteSectionId);
            if (res)
                return JsonStatusResponse.Success("CheckBox has been created successfully");
            return JsonStatusResponse.Error("CheckBox was not created");
        }

}
