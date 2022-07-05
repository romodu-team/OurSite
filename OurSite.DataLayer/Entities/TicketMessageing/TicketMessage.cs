using OurSite.DataLayer.Entities.Accounts;
using OurSite.DataLayer.Entities.BaseEntities;
using OurSite.DataLayer.Entities.Ticketing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.DataLayer.Entities.TicketMessageing
{
    public class TicketMessage : BaseEntity
    {
        #region Properties
        public string MessageText { get; set; }
        public string? SubmittedTicketFileName { get; set; }
        public bool IsSeen { get; set; }
        public long UserOrAdminId { get; set; }
        public long TicketId { get; set; }
        #endregion

        #region Realation
       
        public Ticket Ticket { get; set; }
        
        #endregion
    }
}
