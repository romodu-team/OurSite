using System;
using OurSite.Core.DTOs.Paging;

namespace OurSite.Core.DTOs.Payment
{
    public class ResFilterPayDto : BasePaging
    {
        public List<GetAllPayDto>? Pay { get; set; }
        public ResFilterPayDto SetPaging(BasePaging paging)
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
        public ResFilterPayDto SetPay(List<GetAllPayDto> pay)
        {
            this.Pay = pay;
            return this;
        }
    }
}

