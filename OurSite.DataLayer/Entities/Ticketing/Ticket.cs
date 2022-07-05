using OurSite.DataLayer.Entities.Accounts;
using OurSite.DataLayer.Entities.BaseEntities;
using OurSite.DataLayer.Entities.Departments;
using OurSite.DataLayer.Entities.TicketMessageing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.DataLayer.Entities.Ticketing
{
    public class Ticket : BaseEntity
    {
        #region Properties
        public string TicketTitle { get; set; }
        public string TicketSubject { get; set; }
        public bool IsClosed { get; set; }
        public long DepartmentId { get; set; }
        public long? UserId { get; set; }
       
        #endregion

        #region Realation
        public List<TicketMessage> TicketMessages { get; set; }
        public Department Department { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
     

        #endregion
    }


}
