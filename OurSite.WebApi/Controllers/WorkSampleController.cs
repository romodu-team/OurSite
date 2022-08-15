using Microsoft.AspNetCore.Mvc;
using OurSite.Core.DTOs.WorkSampleDtos;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Utilities;
using OurSite.DataLayer.Entities.RatingModel;

namespace OurSite.WebApi.Controllers.AdminControllers;

 [Route("api/[controller]")]
 [ApiController]
public class WorkSampleController: Controller
{
    #region constructor
    private IWorkSampleService _workSampleService;
    private IWorkSampleCategoryService _workSampleCategoryService;
    public WorkSampleController(IWorkSampleService workSampleService, IWorkSampleCategoryService workSampleCategoryService)
    {
        _workSampleService = workSampleService;
        _workSampleCategoryService = workSampleCategoryService;
    }
    #endregion

    /// <summary>
    /// Creates a work sample
    /// </summary>
    /// <remarks>ShortDescription and Content can contain html. ProjectName is required - The file size must be less than 10 MB</remarks>
    /// <returns></returns>
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
    /// <summary>
    /// Get a work sample. Returns not found or the instance
    /// </summary>
    /// <param name="WorkSampleId"></param>
    /// <returns></returns>
    [HttpGet("get-worksample/{WorkSampleId}")]
    public async Task<IActionResult> GetWorkSample([FromRoute]long WorkSampleId)
    {
        var res= await _workSampleService.GetWorkSample(WorkSampleId);
        if(res is null)
            return JsonStatusResponse.NotFound("Worksample NotFound");
        return JsonStatusResponse.Success(res,"Success");
    }
    /// <summary>
    /// Get the list of filtered orkSamples
    /// </summary>
    /// <remarks>Orderby: DateAsc=0,DateDec=1,LikeAsc=2,LikeDec=3</remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("GetAll-WorkSamples")]
    public async Task<IActionResult> GetAllWorkSamples([FromBody] ReqFilterWorkSampleDto request)
    {
        var res = await _workSampleService.GetAllWorkSamples(request);
        if(res.WorkSamples is not null )
            return JsonStatusResponse.Success(res,"success");
        else
            return JsonStatusResponse.NotFound("No work samples found");
    }
    /// <summary>
    /// create a category
    /// </summary>
    /// <param name="Title"></param>
    /// <param name="Name"></param>
    /// <returns></returns>
    [HttpPost("Create-WorkSample-Category")]
    public async Task<IActionResult> CreateWorkSampleCategory([FromQuery]string Title,[FromQuery]string Name)
    {
        var res = await _workSampleCategoryService.AddCategory(Title,Name);
        if(res)
            return JsonStatusResponse.Success("category has been added successfully");
        return JsonStatusResponse.Error("faild");
    }
    /// <summary>
    /// delete a category
    /// </summary>
    /// <remarks>Deleting a category removes the category from all work samples</remarks>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    [HttpDelete("delete-WorkSample-Category/{categoryId}")]
    public async Task<IActionResult> DeleteWorkSampleCategory([FromRoute]long categoryId)
    {
        var res = await _workSampleCategoryService.DeleteCategory(categoryId);
        if(res)
            return JsonStatusResponse.Success("category has been deleted successfully");
        return JsonStatusResponse.Error("faild");
    }
    /// <summary>
    /// update a category
    /// </summary>
    /// <remarks>Submit only the fields you want to update</remarks>
    /// <param name="CategoryId"></param>
    /// <param name="Title"></param>
    /// <param name="Name"></param>
    /// <returns></returns>
    [HttpPut("Update-WorkSample-Category")]
    public async Task<IActionResult> CreateWorkSampleCategory([FromQuery] long CategoryId,[FromQuery]string? Title,[FromQuery]string? Name)
    {
        var res = await _workSampleCategoryService.Editcategory(CategoryId,Title,Name);
        if(res)
            return JsonStatusResponse.Success("category has been updated successfully");
        return JsonStatusResponse.Error("faild");
    }  
    /// <summary>
    /// Get list of categories
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetAll-WorkSample-Categories")]
    public async Task<IActionResult> GetAllWorkSampleCategories()
    {
        var res = await _workSampleCategoryService.GetAllCategories();
        if(res.Any())
            return JsonStatusResponse.Success(res,"Success");
        return JsonStatusResponse.Error("No categories found");
    }
    /// <summary>
    /// Get a category by id
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    [HttpGet("Get-WorkSample-Category/{categoryId}")]
    public async Task<IActionResult> GetAllWorkSampleCategories([FromRoute]long categoryId)
    {
        var res = await _workSampleCategoryService.GetCategory(categoryId);
        if(res is not null)
            return JsonStatusResponse.Success(res,"Success");
        return JsonStatusResponse.Error("Category not found");
    }
    // we dont need this method
    //[HttpPost("Add-Categories-ToWorkSample/{worksampleId}")]
    //public async Task<IActionResult> AddCategoriesToWorkSample([FromRoute]long worksampleId,[FromBody]List<long> CategoriesId)
    //{
    //    var res =await _workSampleCategoryService.AddCategoriesToWorkSample(worksampleId,CategoriesId);
    //    if(res)
    //        return JsonStatusResponse.Success("category has been added to Worksample successfully");
    //    return JsonStatusResponse.Error("faild");
    //}

    /// <summary>
    /// delete a work sample
    /// </summary>
    /// <remarks>this method delete worksample,delete worksample image gallery,delete worksample like,delete worksample in category,delete project features</remarks>
    /// <param name="worksampleId"></param>
    /// <returns></returns>
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
    /// <summary>
    /// Update work sample
    /// </summary>
    /// <param name="worksampleId"></param>
    /// <param name="request"></param>
    /// <remarks>The file size must be less than 10 MB</remarks>
    /// <returns></returns>
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

    /// <summary>
    /// Api for add like to work sample {Get request from query}
    /// </summary>
    /// <param name="userIp"></param>
    /// <param name="workSampleId"></param>
    /// <returns></returns>
    [HttpGet("add-like")]
    public async Task<IActionResult> AddLike([FromQuery]string userIp,[FromQuery] long workSampleId)
    {
        var like = await _workSampleService.AddLike(userIp, workSampleId);
        switch (like)
        {
            case ResLike.success:
                return JsonStatusResponse.Success("Like Add Successfully");
            case ResLike.Faild:
                return JsonStatusResponse.Error("Server Error");
            case ResLike.Exist:
                return JsonStatusResponse.Error("You like this worksample before");
            case ResLike.WorkSampleNotFound:
                return JsonStatusResponse.NotFound("work sample not found. try agian later.");
            default:
                return JsonStatusResponse.Error("Server Error");
        }
    }
}
