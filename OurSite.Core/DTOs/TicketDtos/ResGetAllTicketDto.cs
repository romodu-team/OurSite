using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.TicketDtos
{
    public class ResGetAllTicketDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string? AssignedTo { get; set; }
        public string? Priority { get; set; }
        public string Status { get; set; }
        public string UserEmail { get; set; }
        public string CreateDate { get; set; }
        public string LastUpdateDate { get; set; }
    }
}
