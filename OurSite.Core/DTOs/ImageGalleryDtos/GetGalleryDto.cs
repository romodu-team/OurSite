namespace OurSite.Core.DTOs.ImageGalleryDtos;

public class GetGalleryDto
{
   // public long GalleryId { get; set; }
    // public List<ImageDto> Images { get; set; }
     public long Id { get; set; }
    public string ImageName { get; set; }
    public string ImagePath { get; set; }
}
public enum ResDeleteFile{
    Success,
    NotFound,
    Faild
}
public enum ResAddImageToGallery{
    Success,
    Failure,
    ToBig,
    NoContent,
    InvalidExtention,
    worksampleNotFound,
    SiteSectionNotValid
}