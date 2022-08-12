using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.TicketDtos
{
    public class TicketPriorityDto
    {
        public long PriorityId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
    }
}
