using Microsoft.AspNetCore.Http;
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
        public bool IsClosed { get; set; }
        public long DepartmentId { get; set; }
        public long UserId { get; set; }
        public string MessageText { get; set; }
        public string? SubmittedTicketFileName { get; set; }
        public IFormFile? SubmittedTicketFile { get; set; }

    }
    public enum ResTicket
    {
        Success,
        Failed
        
    }
}
