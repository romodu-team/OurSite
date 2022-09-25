using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using OurSite.DataLayer.Entities.Projects;

namespace OurSite.Core.DTOs.ProjectDtos
{
    public class EditProjectDto
    {
        [Required]
        public long ProId { get; set; }
        public string? Name { get; set; }
        public ProType? Type { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public float? Price { get; set; }
        public string? Description { get; set; }
        public situations? Situation { get; set; }
        public List<long>? PlanDetails { get; set; }
        public long? AdminId { get; set; }
        


    }


}

