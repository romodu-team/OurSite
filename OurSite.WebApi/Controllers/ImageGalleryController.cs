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
    public async Task<IActionResult> AddImageToGallery([FromForm] ReqAddImageToGallery request)
    {
        var result = await imageGalleryService.AddImageToGallery(request.SiteSection,request.WorkSampleId,request.Image,request.ImageAlt);
        switch (result)
        {
            case ResAddImageToGallery.Success:
                return JsonStatusResponse.Success(message:"image has been successfully added" , ReturnData: request);
            case ResAddImageToGallery.ToBig:
                return JsonStatusResponse.Error("image size is too big");
            case ResAddImageToGallery.Failure:
                return JsonStatusResponse.Error("server error");
            case ResAddImageToGallery.InvalidExtention:
                return JsonStatusResponse.Error("image extention file is invalid");
            case ResAddImageToGallery.NoContent:
                return JsonStatusResponse.Error("image file not found");
            case ResAddImageToGallery.worksampleNotFound:
                return JsonStatusResponse.Error("worksample not found");
            case ResAddImageToGallery.SiteSectionNotValid:
                return JsonStatusResponse.Error("Site Section is Not Valid");

            default:
                return JsonStatusResponse.Error("server error");
        }
    }
    /// <summary>
    /// Delete a image from a site section by image id(its not physical delete)
    /// </summary>
    /// <param name="ImageId"></param>
    /// <returns></returns>
    [HttpDelete("Delete-image-from-gallery")]
    public async Task<IActionResult> DeleteImageFromGallery(long ImageId)
    {
        var result = await imageGalleryService.DeleteImageFromGallery(ImageId);
        switch (result)
        {
            case ResDeleteImage.Success:
                return JsonStatusResponse.Success(message:"image has been successfully Deleted", ReturnData: ImageId);
            case ResDeleteImage.NotFound:
                return JsonStatusResponse.Error("image not found");
            case ResDeleteImage.Faild:
                return JsonStatusResponse.Error("server error");



            default:
                return JsonStatusResponse.Error("server error");
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
            if(result.Any())
                return JsonStatusResponse.Success(result,"successfull");
            else
                return JsonStatusResponse.Error("there is no image");
        }
        return JsonStatusResponse.Error("site section invalid");
    }
}
