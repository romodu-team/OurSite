using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.TicketDtos
{
    public class TicketReportDto
    {
        public int TotalTicket { get; set; }
        public int TotalSolvedTicket { get; set; }
        public int TotalUser { get; set; }
        public Dictionary<string,string>? NumberOfTicketsPerStatus { get; set; }
    }
}
