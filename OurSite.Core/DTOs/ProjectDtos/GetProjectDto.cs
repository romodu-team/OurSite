using System;
using OurSite.DataLayer.Entities.ConsultationRequest;
using OurSite.DataLayer.Entities.Projects;

namespace OurSite.Core.DTOs.ProjectDtos
{
    public class GetProjectDto
    {
        public long ProId { get; set; }
        public string Name { get; set; }
        public ProType Type { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public float? Price { get; set; }
        public string Description { get; set; }
        public situations Situation { get; set; }
        public List<CheckBoxs>? PlanDetails { get; set; }
        public string? ContractFileName { get; set; }
    }
}

