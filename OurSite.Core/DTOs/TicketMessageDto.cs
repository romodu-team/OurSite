using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs
{
    public class TicketMessageDto
    {
        public string MessageText { get; set; }
        public string? SubmittedTicketFileName { get; set; }
        public long UserOrAdminId { get; set; }
        public long? TicketId { get; set; }
        public IFormFile? SubmittedTicketFile { get; set; }

    }
}
