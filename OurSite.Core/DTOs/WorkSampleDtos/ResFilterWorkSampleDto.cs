using OurSite.Core.DTOs.Paging;

namespace OurSite.Core.DTOs.WorkSampleDtos;

public class ResFilterWorkSampleDto:BasePaging
{
     public List<GetAllWorkSampleDto>? WorkSamples { get; set; }

        public ResFilterWorkSampleDto SetPaging(BasePaging paging)
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
        public ResFilterWorkSampleDto SetWorkSamples(List<GetAllWorkSampleDto> WorkSamples)
        {
            this.WorkSamples = WorkSamples;
            return this;
        }
}