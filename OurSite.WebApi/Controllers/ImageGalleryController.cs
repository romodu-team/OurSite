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
    [HttpPost("add-image-gallery")]
    public async Task<IActionResult> AddImageToGallery([FromForm] ReqAddImageToGallery request)
    {
        var result = await imageGalleryService.AddImageToGallery(request.SiteSection,request.WorkSampleId,request.Image,request.ImageAlt);
        switch (result)
        {
            case ResAddImageToGallery.Success:
                return JsonStatusResponse.Success("image has been successfully added");
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
    [HttpDelete("Delete-image-from-gallery")]
    public async Task<IActionResult> DeleteImageFromGallery(long ImageId)
    {
        var result = await imageGalleryService.DeleteImageFromGallery(ImageId);
        switch (result)
        {
            case ResDeleteImage.Success:
                return JsonStatusResponse.Success("image has been successfully Deleted");
            case ResDeleteImage.NotFound:
                return JsonStatusResponse.Error("image not found");
            case ResDeleteImage.Faild:
                return JsonStatusResponse.Error("server error");



            default:
                return JsonStatusResponse.Error("server error");
        }
    }

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
