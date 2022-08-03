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
    private IWorkSampleCategoryService _workSampleCategoryService;
    public WorkSampleController(IWorkSampleService workSampleService,IWorkSampleCategoryService workSampleCategoryService)
    {
        _workSampleService=workSampleService;
        _workSampleCategoryService=workSampleCategoryService;
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

    [HttpPost("GetAll-WorkSamples")]
    public async Task<IActionResult> GetAllWorkSamples([FromBody] ReqFilterWorkSampleDto request)
    {
        var res = await _workSampleService.GetAllWorkSamples(request);
        if(res.WorkSamples is not null )
            return JsonStatusResponse.Success(res,"success");
        else
            return JsonStatusResponse.NotFound("No work samples found");
    }
    
    [HttpPost("Create-WorkSample-Category")]
    public async Task<IActionResult> CreateWorkSampleCategory([FromQuery]string Title,[FromQuery]string Name)
    {
        var res = await _workSampleCategoryService.AddCategory(Title,Name);
        if(res)
            return JsonStatusResponse.Success("category has been added successfully");
        return JsonStatusResponse.Error("faild");
    }

    [HttpDelete("delete-WorkSample-Category/{categoryId}")]
    public async Task<IActionResult> DeleteWorkSampleCategory([FromRoute]long categoryId)
    {
        var res = await _workSampleCategoryService.DeleteCategory(categoryId);
        if(res)
            return JsonStatusResponse.Success("category has been deleted successfully");
        return JsonStatusResponse.Error("faild");
    }

    [HttpPut("Update-WorkSample-Category")]
    public async Task<IActionResult> CreateWorkSampleCategory([FromQuery] long CategoryId,[FromQuery]string? Title,[FromQuery]string? Name)
    {
        var res = await _workSampleCategoryService.Editcategory(CategoryId,Title,Name);
        if(res)
            return JsonStatusResponse.Success("category has been updated successfully");
        return JsonStatusResponse.Error("faild");
    }  

    [HttpGet("GetAll-WorkSample-Categories")]
    public async Task<IActionResult> GetAllWorkSampleCategories()
    {
        var res = await _workSampleCategoryService.GetAllCategories();
        if(res.Any())
            return JsonStatusResponse.Success(res,"Success");
        return JsonStatusResponse.Error("No categories found");
    }

    [HttpGet("Get-WorkSample-Category/{categoryId}")]
    public async Task<IActionResult> GetAllWorkSampleCategories([FromRoute]long categoryId)
    {
        var res = await _workSampleCategoryService.GetCategory(categoryId);
        if(res is not null)
            return JsonStatusResponse.Success(res,"Success");
        return JsonStatusResponse.Error("Category not found");
    }  

    [HttpPost("Add-Categories-ToWorkSample/{worksampleId}")]
    public async Task<IActionResult> AddCategoriesToWorkSample([FromRoute]long worksampleId,[FromBody]List<long> CategoriesId)
    {
        var res =await _workSampleCategoryService.AddCategoriesToWorkSample(worksampleId,CategoriesId);
        if(res)
            return JsonStatusResponse.Success("category has been added to Worksample successfully");
        return JsonStatusResponse.Error("faild");
    }

    [HttpDelete("delete-WorkSample/{worksampleId}")]
    public async Task<IActionResult> DeleteWorkSample([FromRoute] long worksampleId)
    {
        var res = await _workSampleService.DeleteWorkSample(worksampleId);
        switch (res)
        {
            case ResWorkSample.Success:
                return JsonStatusResponse.Success("work sample has been deleted successfully");
            case ResWorkSample.Faild:
                return JsonStatusResponse.Error("faild");
            default:
                return JsonStatusResponse.Error("server error");
        }
    }

    [HttpPut("update-workSample/{worksampleId}")]
    public async Task<IActionResult> UpdateWorkSample([FromRoute]long worksampleId,[FromForm]EditWorkSampleDto request)
    {
        var res = await _workSampleService.EditWorkSample(worksampleId,request);
        switch (res)
        {
            case ResWorkSample.Success:
                return JsonStatusResponse.Success("The Work sample has been successfully updated");
            case ResWorkSample.Faild:
                return JsonStatusResponse.Error("Faild");

            case ResWorkSample.NotFound:
                return JsonStatusResponse.NotFound("Work sample Not found");

            default:
                return JsonStatusResponse.Error("Server Error");

        }
    }
}
