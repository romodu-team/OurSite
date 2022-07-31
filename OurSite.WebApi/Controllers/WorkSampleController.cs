using Microsoft.AspNetCore.Mvc;
using OurSite.Core.DTOs.WorkSampleDtos;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Utilities;

namespace OurSite.WebApi.Controllers.AdminControllers;

 [Route("api/[controller]")]
 [ApiController]
public class WorkSampleController: Controller
{
    private IWorkSampleService _workSampleService;
    public WorkSampleController(IWorkSampleService workSampleService)
    {
        _workSampleService=workSampleService;
    }
    [HttpPost("create-WorkSample")]
    public async Task<IActionResult> CreateWorkSample([FromForm]CreateWorkSampleDto request){
        if(ModelState.IsValid){
            var res =await _workSampleService.CreateWorkSample(request);
            if(res.WorkSampleID != -1)
                 return JsonStatusResponse.Success(res.WorkSampleID,"successfull");
            else
                return JsonStatusResponse.Error(res,"server error");
        }
        return JsonStatusResponse.Error("model state is not valid");
    }

    [HttpGet("get-worksample/{WorkSampleId}")]
    public async Task<IActionResult> GetWorkSample([FromRoute]long WorkSampleId)
    {
        var res= await _workSampleService.GetWorkSample(WorkSampleId);
        if(res is null)
            return JsonStatusResponse.NotFound("Worksample NotFound");
        return JsonStatusResponse.Success(res,"Success");
    }

    [HttpGet("GetAll-WorkSamples")]
    public async Task<IActionResult> GetAllWorkSamples([FromQuery] ReqFilterWorkSampleDto request)
    {
        var res = await _workSampleService.GetAllWorkSamples(request);
        if(res.WorkSamples is not null )
            return JsonStatusResponse.Success(res,"success");
        else
            return JsonStatusResponse.NotFound("No work samples found");
    }
}
