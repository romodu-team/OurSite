using System;
using OurSite.DataLayer.Entities.Projects;

namespace OurSite.Core.DTOs.ProjectDtos
{
    public class EditProjectDto
    {
        public string Name { get; set; }
        public ProType Type { get; set; }
        public DateTime dateTime { get; set; }
        public DateTime EndTime { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public situations Situation { get; set; }
        public string PlanDetails { get; set; }
        public long AdminId { get; set; }
        public long UserId { get; set; }
        public string ContractFileName { get; set; }



    }


}

