namespace OurSite.Core.DTOs.ImageGallery;

public class GetGalleryDto
{
   // public long GalleryId { get; set; }
    // public List<ImageDto> Images { get; set; }
     public long Id { get; set; }
    public string ImageName { get; set; }
    public string ImagePath { get; set; }
}
public enum ResDeleteImage{
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
    worksampleNotFound
}