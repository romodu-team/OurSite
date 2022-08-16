using OurSite.Core.DTOs.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.ConsultationRequestDtos
{
    public class ResFilterConsultationRequestDto : BasePaging
    {
        public List<GetAllConsultationRequestDto>? ConsultationRequests { get; set; }

        public ResFilterConsultationRequestDto SetPaging(BasePaging paging)
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
        public ResFilterConsultationRequestDto SetConsultationRequests(List<GetAllConsultationRequestDto> consultationRequests)
        {
            this.ConsultationRequests = consultationRequests;
            return this;
        }
    }
}

