using OurSite.DataLayer.Entities.BaseEntities;
using OurSite.OurSite.DataLayer.Entities.WorkSamples;

namespace OurSite.DataLayer.Entities.WorkSamples;

public class WorkSampleCategory:BaseEntity
{
    public string Title { get; set; }
    public string Name { get; set; }

    public ICollection<WorkSampleInCategory>? workSampleInCategories{get;set;}
     
}