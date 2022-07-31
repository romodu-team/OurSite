using System.ComponentModel.DataAnnotations.Schema;
using OurSite.DataLayer.Entities.BaseEntities;
using OurSite.DataLayer.Entities.WorkSamples;

namespace OurSite.OurSite.DataLayer.Entities.WorkSamples;
public class WorkSampleInCategory:BaseEntity
{
    public long WorkSampleId { get; set; }
    public long WorkSampleCategoryId { get; set; }

    [ForeignKey("WorkSampleId")]
    public WorkSample WorkSample { get; set; }
    [ForeignKey("WorkSampleCategoryId")]
    public WorkSampleCategory workSampleCategory { get; set; }
}