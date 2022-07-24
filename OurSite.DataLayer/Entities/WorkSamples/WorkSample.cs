using OurSite.DataLayer.Entities.BaseEntities;
using OurSite.DataLayer.Entities.Comments;
using OurSite.DataLayer.Entities.ImageGalleries;
using OurSite.DataLayer.Entities.RatingModel;

namespace OurSite.DataLayer.Entities.WorkSamples;

public class WorkSample:BaseEntity
{
    public string ProjectName { get; set; }
    public string EmployerName { get; set; }
    public string CustomUrl { get; set; }
    public string ShortDescription { get; set; }
    public string LogoImageName { get; set; }
    public string HeaderImageName { get; set; }
    public string Description { get; set; }



    public ICollection<ProjectFeatures>? ProjectFeatures { get; set; }
    public ICollection<ImageGallery>? ImageGalleries { get; set; }
    public ICollection<Like>? Likes { get; set; }
    public ICollection<Comment>? Comments { get; set; }
}
