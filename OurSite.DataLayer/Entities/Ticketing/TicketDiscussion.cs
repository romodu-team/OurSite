using OurSite.DataLayer.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.DataLayer.Entities.Ticketing
{
    public class TicketDiscussion:BaseEntity
    {
        public long TicketId { get; set; }
        public long SenderId { get; set; }
        public string Content { get; set; }
        public long? AttachmentId { get; set; }
        public TicketModel Ticket { get; set; }
        public TicketAttachment Attachment { get; set; }
    }
}
