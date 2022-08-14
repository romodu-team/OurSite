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
        public async Task<IActionResult> CreateCheckBox(ReqCreateCheckBoxDto request)
        {
            var res = await _CheckBoxService.CreateCheckBox(request.Title,request.IconName,request.Description,request.SiteSectionId);
            if (res)
                return JsonStatusResponse.Success("CheckBox has been created successfully");
            return JsonStatusResponse.Error("CheckBox was not created");
        }
        /// <summary>
        /// delete CheckBox
        /// </summary>
        /// <param name="CheckBoxId"></param>
        /// <returns></returns>
        [HttpDelete("Delete-CheckBox")]
        public async Task<IActionResult> DeleteCheckBox([FromBody]List<long> CheckBoxId)
        {
            var res = await _CheckBoxService.DeleteCheckBox(CheckBoxId);
            switch (res)
            {
                case Core.DTOs.TicketDtos.ResDeleteOpration.Success:
                    return JsonStatusResponse.Success("CheckBox has been deleted successfully");
                case Core.DTOs.TicketDtos.ResDeleteOpration.Failure:
                    return JsonStatusResponse.Error("CheckBox was not deleted");
                case Core.DTOs.TicketDtos.ResDeleteOpration.RefrenceError:
                    return JsonStatusResponse.Error("You cannot delete this CheckBox because already in use");
                default:
                    return JsonStatusResponse.Error("CheckBox was not deleted");
            }
        }
        /// <summary>
        /// get a CheckBox details
        /// </summary>
        /// <param name="CheckBoxId"></param>
        /// <returns></returns>
        [HttpGet("Get-CheckBox")]
        public async Task<IActionResult> GetCheckBox(long CheckBoxId)
        {
            var res = await _CheckBoxService.GetCheckBox(CheckBoxId);
            if (res is not null)
                return JsonStatusResponse.Success(res,"successfull");
            return JsonStatusResponse.Error("CheckBox not found");
        }
        /// <summary>
        /// update CheckBox , Enter the fields you want to update
        /// </summary>
        /// <param name="request"></param>
        [HttpPut("Update-CheckBox")]
        public async Task<IActionResult> UpdateCheckBox([FromBody]ReqUpdateCheckBox request)
        {
            var res = await _CheckBoxService.UpdateCheckBox(request.CheckBoxId,request.Title,request.IconName,request.Description, (int?)request.SiteSectionId);
            if (res)
                return JsonStatusResponse.Success("CheckBox has been updated successfully");
            return JsonStatusResponse.Error("CheckBox was not updated");
        }
        /// <summary>
        /// get list of CheckBox
        /// </summary>
        [HttpGet("get-all-CheckBox")]
        public async Task<IActionResult> GetAllCheckBox()
        {
            var res = await _CheckBoxService.GetAllCheckBox();
            if (res is not null && res.Count>0)
                return JsonStatusResponse.Success(res,"successfull");
            return JsonStatusResponse.Error("no CheckBox found");
        }
}
