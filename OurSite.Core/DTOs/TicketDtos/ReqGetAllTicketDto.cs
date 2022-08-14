using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.TicketDtos
{
    public class ReqGetAllTicketDto
    {
        public int PageId { get; set; }
        public int TakeEntity { get; set; }
        public string? TicketName { get; set; }
        public string? UserNameOrUserEmail { get; set; }
        public long? StatusId { get; set; }
        public long? PriorityId { get; set; }
        public long? CategoryId { get; set; }
        public OrderByDate? OrderByDate { get; set; }
    }
    public enum OrderByDate
    {
        CreateDateAsc,
        CreateDateDesc,
        LastUpdateDateAsc,
        LastUpdateDateDesc
    }
}
