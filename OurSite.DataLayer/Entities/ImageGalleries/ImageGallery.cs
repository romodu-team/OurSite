using OurSite.DataLayer.Entities.BaseEntities;

namespace OurSite.DataLayer.Entities.ImageGalleries;

public class ImageGallery:BaseEntity
{
    public string ImageName { get; set; }
    public string ImagePath { get; set; }
    public SiteSections SiteSection { get; set; }
    public long? SectionId { get; set; }
}
public enum SiteSections{
    WorkSamples,
    HomePage,
    AboutUs
}
