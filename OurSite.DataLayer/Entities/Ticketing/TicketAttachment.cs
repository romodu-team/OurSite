using OurSite.DataLayer.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.DataLayer.Entities.Ticketing
{
    public class TicketAttachment:BaseEntity
    {
        public long TicketDiscussionId { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string ContentType { get; set; }

        public TicketDiscussion TicketDiscussion { get; set; }

    }
}
