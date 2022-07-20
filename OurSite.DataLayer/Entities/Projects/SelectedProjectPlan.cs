using OurSite.DataLayer.Entities.BaseEntities;
using OurSite.DataLayer.Entities.ConsultationRequest;

namespace OurSite.DataLayer.Entities.Projects;

public class SelectedProjectPlan:BaseEntity
{
    public long ProjectId { get; set; }
    public long CheckBoxId { get; set; }

    public Project Project { get; set; }
    public CheckBoxs CheckBox { get; set; }
}
