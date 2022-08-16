using OurSite.DataLayer.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.DataLayer.Entities.Ticketing
{
    public class TicketAttachment:BaseEntity
    {
        public long TicketDiscussionId { get; set; }
        public string FileName { get; set; }
        public float FileSize { get; set; }
        public string ContentType { get; set; }
        [ForeignKey("TicketDiscussionId")]
        public TicketDiscussion TicketDiscussion { get; set; }

    }
}
