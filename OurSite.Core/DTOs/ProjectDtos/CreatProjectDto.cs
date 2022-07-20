﻿using System;
using OurSite.DataLayer.Entities.Projects;

namespace OurSite.Core.DTOs.ProjectDtos
{
    public class CreatProjectDto
    {
        public string Name { get; set; }
        public ProType Type { get; set; }
        public string Description { get; set; }
        public string? PlanDetails { get; set; }

        

        public enum ResProject
        {
            Success,
            Faild,
            Error,
            SitutionError,
            NotFound,
            InvalidInput
        }
    }
}

