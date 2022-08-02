using System;
using OurSite.DataLayer.Entities.Payments;

namespace OurSite.Core.DTOs.Payment
{
    public class ReqFilterPayDto
    {
        public int PageId { get; set; }
        public int TakeEntity { get; set; }
        public string? UserName { get; set; }
        public PayDateOrderBy? CreatDateOrderBy { get; set; }
        public StatusPay? StatusPay { get; set; }
        public PayRemoveFilter? RemoveFilter { get; set; }

    }
    public enum PayDateOrderBy
    {
        CreateDateAsc,
        CreateDateDec,
        DatePayAsc,
        DatePayDec
    }

    public enum PayRemoveFilter
    {
        Deleted,
        NotDeleted,
        All
    }
}


