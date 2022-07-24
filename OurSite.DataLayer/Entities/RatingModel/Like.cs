using System.ComponentModel.DataAnnotations;
using OurSite.DataLayer.Entities.BaseEntities;
using OurSite.DataLayer.Entities.WorkSamples;

namespace OurSite.DataLayer.Entities.RatingModel;

public class Like:BaseEntity
{
    public long WorkSampleId { get; set; }
    public WorkSample WorkSample { get; set; }
}
