using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OurSite.Core.DTOs.CheckBoxDtos;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Utilities;

namespace OurSite.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CheckBoxController : Controller
{
    #region Constructor
    private ICheckBoxService _CheckBoxService;
    public CheckBoxController(ICheckBoxService CheckBoxService)
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
    [Authorize(Policy =StaticPermissions.PermissionManageCheckBox)]
    public async Task<IActionResult> CreateCheckBox(ReqCreateCheckBoxDto request)
    {
        var res = await _CheckBoxService.CreateCheckBox(request.Title, request.IconName, request.Description, request.SiteSectionId);
        if (res)
        {
            HttpContext.Response.StatusCode = 201;
            return JsonStatusResponse.Success("CheckBox has been created successfully");
        }
        HttpContext.Response.StatusCode = 500;
        return JsonStatusResponse.Error("CheckBox was not created");
    }
    /// <summary>
    /// delete CheckBoxes
    /// </summary>
    /// <param name="CheckBoxId"></param>
    /// <returns></returns>
    [HttpDelete("Delete-CheckBox")]
    [Authorize(Policy = StaticPermissions.PermissionManageCheckBox)]

    public async Task<IActionResult> DeleteCheckBox([FromBody] List<long> CheckBoxId)
    {
        var res = await _CheckBoxService.DeleteCheckBox(CheckBoxId);
        switch (res)
        {
            case Core.DTOs.TicketDtos.ResDeleteOpration.Success:
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success("CheckBox has been deleted successfully");
            case Core.DTOs.TicketDtos.ResDeleteOpration.Failure:
                HttpContext.Response.StatusCode = 500;
                return JsonStatusResponse.Error("CheckBox was not deleted");
            case Core.DTOs.TicketDtos.ResDeleteOpration.RefrenceError:
                HttpContext.Response.StatusCode = 409;
                return JsonStatusResponse.Error("You cannot delete this CheckBox because already in use");
            default:
                HttpContext.Response.StatusCode = 500;
                return JsonStatusResponse.UnhandledError();
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
        {
            HttpContext.Response.StatusCode = 200;
            return JsonStatusResponse.Success(res, "successfull");
        }
        HttpContext.Response.StatusCode = 404;
        return JsonStatusResponse.NotFound("CheckBox not found");
    }
    /// <summary>
    /// update CheckBox , Enter the fields you want to update
    /// </summary>
    /// <param name="request"></param>
    [HttpPut("Update-CheckBox")]
    [Authorize(Policy = StaticPermissions.PermissionManageCheckBox)]

    public async Task<IActionResult> UpdateCheckBox([FromBody] ReqUpdateCheckBox request)
    {
        var res = await _CheckBoxService.UpdateCheckBox(request.CheckBoxId, request.Title, request.IconName, request.Description, (int?)request.SiteSectionId);
        if (res)
        {
            HttpContext.Response.StatusCode = 200;
            return JsonStatusResponse.Success("CheckBox has been updated successfully");
        }
        HttpContext.Response.StatusCode = 500;
        return JsonStatusResponse.Error("CheckBox was not updated");
    }
    /// <summary>
    /// get list of CheckBox
    ///<remarks>sectionId=> ConsultationForm = 0,PlanProject = 1</remarks>
    /// </summary>
    [HttpGet("get-all-CheckBox")]
    public async Task<IActionResult> GetAllCheckBox([FromQuery]string sectionId)
    {
        var res = await _CheckBoxService.GetAllCheckBox(sectionId);
        if (res is not null && res.Count > 0)
        {
            HttpContext.Response.StatusCode = 200;
            return JsonStatusResponse.Success(res, "successfull");
        }
        HttpContext.Response.StatusCode = 404;
        return JsonStatusResponse.NotFound("no CheckBox found");
    }
}
