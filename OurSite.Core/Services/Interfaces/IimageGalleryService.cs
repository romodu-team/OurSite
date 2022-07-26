using Microsoft.AspNetCore.Http;
using OurSite.Core.DTOs.ImageGalleryDtos;
using OurSite.DataLayer.Entities.ImageGalleries;

namespace OurSite.Core.Services.Interfaces;

public interface IimageGalleryService
{
    Task<List<GetGalleryDto>> GetActiveGalleryByWorkSampleId(long WorkSampleId);
    Task<ResDeleteImage> DeleteImageFromGallery(long ImageId);
    Task<ResAddImageToGallery> AddImageToGallery(SiteSections SiteSection,long WorkSampleId,IFormFile Image,string? alt);
}
