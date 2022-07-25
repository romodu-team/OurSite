using System;
using OurSite.Core.DTOs.Paging;

namespace OurSite.Core.DTOs.ProjectDtos
{
    public class ResFilterProjectDto : BasePaging
    {
        public List<GetAllProjectDto>? Projects { get; set; }
        public ResFilterProjectDto SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.PageCount = paging.PageCount;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.TakeEntity = paging.TakeEntity;
            this.SkipEntity = paging.SkipEntity;
            this.ActivePage = paging.ActivePage;
            return this;
        }
        public ResFilterProjectDto SetUsers(List<GetAllProjectDto> projects)
        {
            this.Projects = projects;
            return this;
        }

    }
}

