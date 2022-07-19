using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.TicketsDtos
{
    public class ResViewTicketDto
    {
        public long TicketId { get; set; }
        public string TicketTitle { get; set; }
        public bool IsClosed { get; set; }
        public long DepartmentId { get; set; }
        public string TicketSubject { get; set; }

    }
}
