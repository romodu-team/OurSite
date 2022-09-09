using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OurSite.Core.DTOs.ImageGalleryDtos;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Utilities;
using OurSite.DataLayer.Entities.ImageGalleries;

namespace OurSite.WebApi.Controllers;

 [Route("api/[controller]")]
 [ApiController]
public class ImageGalleryController:Controller
{
    private IimageGalleryService imageGalleryService;
    public ImageGalleryController(IimageGalleryService imageGalleryService)
    {
        this.imageGalleryService=imageGalleryService;
    }

    /// <summary>
    /// Upload an image for a specific part of the site.
    /// site Sections: Worksample = 0, home page = 1, about us = 2
    /// Returns:
    /// Success("image has been successfully added")
    /// Error("image size is too big")
    /// Error("server error")
    /// Error("image extention file is invalid")
    /// Error("image file not found")
    /// Error("worksample not found")
    /// Error("Site Section is Not Valid")
    /// </summary>
    /// <remarks>The size of the image file must be less than 10 MB</remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("add-image-gallery")]
    [Authorize(Policy = StaticPermissions.PermissionAddImageToGallery)]

    public async Task<IActionResult> AddImageToGallery([FromForm] ReqAddImageToGallery request)
    {
        var result = await imageGalleryService.AddImageToGallery(request.SiteSection,request.WorkSampleId,request.Image,request.ImageAlt);
        switch (result)
        {
            case ResAddImageToGallery.Success:
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success(message:"image has been successfully added" , ReturnData: await imageGalleryService.ReturnImageAdress(request.WorkSampleId));
            case ResAddImageToGallery.ToBig:
                HttpContext.Response.StatusCode = 413;
                return JsonStatusResponse.Error("image size is too big");
            case ResAddImageToGallery.Failure:
                HttpContext.Response.StatusCode = 500;
                return JsonStatusResponse.Error("server error");
            case ResAddImageToGallery.InvalidExtention:
                HttpContext.Response.StatusCode = 400;
                return JsonStatusResponse.Error("image extention file is invalid");
            case ResAddImageToGallery.NoContent:
                HttpContext.Response.StatusCode = 204;
                return JsonStatusResponse.Error("image file not found");
            case ResAddImageToGallery.worksampleNotFound:
                HttpContext.Response.StatusCode = 404;
                return JsonStatusResponse.Error("worksample not found");
            case ResAddImageToGallery.SiteSectionNotValid:
                HttpContext.Response.StatusCode = 400;
                return JsonStatusResponse.Error("Site Section is Not Valid");

            default:
                HttpContext.Response.StatusCode = 500;
                return JsonStatusResponse.UnhandledError();
        }
    }


    /// <summary>
    /// Delete a image from a site section by image id(its not physical delete)
    /// </summary>
    /// <param name="ImageId"></param>
    /// <returns></returns>
    [HttpDelete("Delete-image-from-gallery")]
    [Authorize(Policy = StaticPermissions.PermissionDeleteImageFromGallery)]

    public async Task<IActionResult> DeleteImageFromGallery(long ImageId)
    {
        var result = await imageGalleryService.DeleteImageFromGallery(ImageId);
        switch (result)
        {
            case ResDeleteFile.Success:
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success(message:"image has been successfully Deleted", ReturnData: ImageId);
            case ResDeleteFile.NotFound:
                HttpContext.Response.StatusCode = 404;
                return JsonStatusResponse.Error("image not found");
            case ResDeleteFile.Faild:
                HttpContext.Response.StatusCode = 500;
                return JsonStatusResponse.Error("server error");



            default:
                HttpContext.Response.StatusCode = 500;
                return JsonStatusResponse.UnhandledError();
        }
    }



    /// <summary>
    /// Get all the active images of a section of the site by section Id
    /// return list of images
    /// </summary>
    /// <param name="section"></param>
    /// <param name="SectionId"></param>
    /// <returns></returns>
    [HttpGet("get-gallery")]
    public async Task<IActionResult> GetImageGallery(SiteSections section,long SectionId)
    {
        if(section==SiteSections.WorkSamples){
            var result = await imageGalleryService.GetActiveGalleryByWorkSampleId(SectionId);
            if (result.Any())
            {
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success(result, "successfull");
            }
            else
            {
                HttpContext.Response.StatusCode = 404;
                return JsonStatusResponse.Error("there is no image");
            }
        }
        HttpContext.Response.StatusCode = 400;
        return JsonStatusResponse.Error("site section invalid");
    }
}
