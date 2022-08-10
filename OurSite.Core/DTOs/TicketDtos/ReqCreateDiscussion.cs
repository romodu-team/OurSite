using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.TicketDtos
{
    public class ReqCreateDiscussion
    {
        public long TicketId { get; set; }
        public long SenderId { get; set; }
        public bool IsAdmin { get; set; }
        public string Content { get; set; }
        public IFormFile? Attachment { get; set; }
    }
}
