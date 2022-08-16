using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.TicketDtos
{
    public class ReqUpdateTicketDto
    {
        public long TicketId { get; set; }
        public string? Title { get; set; }
        public long? TicketCategoryId { get; set; }
        public long? TicketStatusId { get; set; }
        public long? TicketPriorityId { get; set; }
        public long? SupporterId { get; set; }
    }
}
