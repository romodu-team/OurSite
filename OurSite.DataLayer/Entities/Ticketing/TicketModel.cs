using OurSite.DataLayer.Entities.Accounts;
using OurSite.DataLayer.Entities.BaseEntities;
using OurSite.DataLayer.Entities.Projects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.DataLayer.Entities.Ticketing
{
    public class TicketModel:BaseEntity
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public long TicketCategoryId { get; set; }
        [Required]
        public long TicketStatusId { get; set; }
        [Required]
        public long? TicketPriorityId { get; set; }
        public long? SupporterId { get; set; }
        [Required]
        public long UserId { get; set; }
        public long? ProjectId { get; set; }

        #region Relations
        public TicketCategory TicketCategory { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public TicketPriority TicketPriority { get; set; }
        public Admin Supporter { get; set; }
        public User User { get; set; }
        public Project Project { get; set; }
        public virtual ICollection<TicketDiscussion> Discussions { get; set; }
        #endregion
    }
}
