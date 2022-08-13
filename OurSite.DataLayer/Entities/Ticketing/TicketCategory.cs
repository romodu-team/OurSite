using OurSite.DataLayer.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.DataLayer.Entities.Ticketing
{
    public class TicketCategory:BaseEntity
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public long? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual TicketCategory TicketParentCategory { get; set; }
    }
}
