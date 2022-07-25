using System;
using OurSite.DataLayer.Entities.Payments;

namespace OurSite.Core.DTOs.Payment
{
    public class ReqFilterPayDto
    {
        public int PageId { get; set; }
        public int TakeEntity { get; set; }
        public string? UserName { get; set; }
        public PayCreatDateOrderBy? CreatDateOrderBy { get; set; }
        public StatusPay? StatusPay { get; set; }
        public PayRemoveFilter? RemoveFilter { get; set; }
        public DateTime DatePay { get; set; }

    }
    public enum PayCreatDateOrderBy
    {
        CreateDateAsc,
        CreateDateDec
    }

    public enum PayRemoveFilter
    {
        Deleted,
        NotDeleted,
        All
    }
}


