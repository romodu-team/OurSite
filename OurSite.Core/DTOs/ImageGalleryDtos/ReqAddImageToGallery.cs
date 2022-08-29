using Microsoft.AspNetCore.Http;
using OurSite.DataLayer.Entities.ImageGalleries;

namespace OurSite.Core.DTOs.ImageGalleryDtos;

public class ReqAddImageToGallery
{
    public IFormFile Image { get; set; }    
    public long WorkSampleId { get; set; }
    public SiteSections SiteSection { get; set; }
    public string? ImageAlt {get;set;}
}
