using OurSite.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.TicketDtos
{
    public class ResUploadTicketAttachment
    {
        public string? FileName { get; set; }
        public long? FileSize { get; set; }
        public string? ContentType { get; set; }
        public resFileUploader Status { get; set; }
    }
}
