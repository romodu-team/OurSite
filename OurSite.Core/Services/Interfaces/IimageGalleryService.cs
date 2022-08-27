using Microsoft.AspNetCore.Http;
using OurSite.Core.DTOs.ImageGalleryDtos;
using OurSite.DataLayer.Entities.ImageGalleries;

namespace OurSite.Core.Services.Interfaces;

public interface IimageGalleryService : IDisposable
{
    Task<List<GetGalleryDto>> GetActiveGalleryByWorkSampleId(long WorkSampleId);
    Task<ResDeleteFile> DeleteImageFromGallery(long ImageId);
    Task<ResAddImageToGallery> AddImageToGallery(SiteSections SiteSection,long WorkSampleId,IFormFile Image,string? alt);
}
