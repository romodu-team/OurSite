using System;
using OurSite.DataLayer.Entities.Projects;

namespace OurSite.Core.DTOs.ProjectDtos
{
    public class ReqFilterProjectDto
    {
        public int PageId { get; set; }
        public int TakeEntity { get; set; }
        public string? UserName { get; set; }
        public ProjectsCreatDateOrderBy? CreatDateOrderBy { get; set; }
        public ProType? TypeOrderBy { get; set; }
        public ProjectRemoveFilter? RemoveFilter { get; set; }
        public situations? SituationsFilter { get; set; }

    }
    public enum ProjectsCreatDateOrderBy
    {
        CreateDateAsc,
        CreateDateDec
    }

    public enum ProjectRemoveFilter
    {
        Deleted,
        NotDeleted,
        All
    }

    
}


