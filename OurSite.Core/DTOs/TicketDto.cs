using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs
{
    public class TicketDto
    {
        public string TicketTitle { get; set; }
        public string TicketSubject { get; set; }
        public bool TicketSatate { get; set; }
        public long DepartmentId { get; set; }
    }
}
