using OurSite.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.TicketDtos
{
    public class ResCreateDiscussionDto
    {
        public resFileUploader AttachmentStatus { get; set; }
        public ResOperation DiscussionStatus { get; set; }
    }
}
