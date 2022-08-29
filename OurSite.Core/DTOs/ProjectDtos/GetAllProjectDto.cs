using System;
using OurSite.DataLayer.Entities.Projects;

namespace OurSite.Core.DTOs.ProjectDtos
{
    public class GetAllProjectDto
    {
        public long ProjectId { get; set; }
        public ProType Type { get; set; }
        public situations Situations { get; set; }
        public string UserName { get; set; }
        public string? CreatDate { get; set; }
        public bool IsRemove { get; set; }
    }
}

