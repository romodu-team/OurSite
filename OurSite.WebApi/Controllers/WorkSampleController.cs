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
            if(res != -1)
                 return JsonStatusResponse.Success(res,"successfull");
            else
                return JsonStatusResponse.Error(res,"server error");
        }
        return JsonStatusResponse.Error("model state is not valid");
    }

}
