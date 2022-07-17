using OurSite.Core.DTOs.ContactWithUs;
using OurSite.Core.DTOs.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.ContactWithUsDtos
{
    public class ResFilterContactWithUsDto : BasePaging
    {
        public List<GetAllContactWithUsDto>? ContactWithUses { get; set; }

        public ResFilterContactWithUsDto SetPaging(BasePaging paging)
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
        public ResFilterContactWithUsDto SetContactWithUses(List<GetAllContactWithUsDto> contactWithUses)
        {
            this.ContactWithUses = contactWithUses;
            return this;
        }
    }
}
