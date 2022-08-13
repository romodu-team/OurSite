using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.TicketDtos
{
    public class ReqGetAllUserTicketsDto
    {
        public int PageId { get; set; }
        public int TakeEntity { get; set; }
        public string? TicketName { get; set; }
        public long? StatusId { get; set; }
        public OrderByDate? OrderByDate { get; set; }

    }
}
