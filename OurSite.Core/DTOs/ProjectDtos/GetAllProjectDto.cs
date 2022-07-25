using System;
using OurSite.DataLayer.Entities.Projects;

namespace OurSite.Core.DTOs.ProjectDtos
{
    public class GetAllProjectDto
    {
        public long ProjectId { get; set; }
        public ProType Type { get; set; }
        public situations Situations { get; set; }
        public object MyProperty { get; set; }
        public string NameUser { get; set; }
        public DateTime CreatDate { get; set; }
    }
}

