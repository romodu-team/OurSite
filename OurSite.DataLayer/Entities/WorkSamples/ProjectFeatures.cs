using System.ComponentModel.DataAnnotations.Schema;
using OurSite.DataLayer.Entities.BaseEntities;

namespace OurSite.DataLayer.Entities.WorkSamples;

public class ProjectFeatures:BaseEntity
{
    public long WorkSampleId { get; set; }
    public string Title { get; set; }
    public WorkSampleFeatureType FeatureType { get; set; }
    [ForeignKey("WorkSampleId")]
    public WorkSample WorkSample { get; set; }
}
public enum WorkSampleFeatureType{
    ProjectFeatured,
    DesignedPages
}
