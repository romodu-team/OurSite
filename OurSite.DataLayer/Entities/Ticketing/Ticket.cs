using OurSite.DataLayer.Entities.BaseEntities;
using OurSite.DataLayer.Entities.Departments;
using OurSite.DataLayer.Entities.TicketMessageing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.DataLayer.Entities.Ticketing
{
    public class Ticket : BaseEntity
    {
        #region Properties
        public string TicketTitle { get; set; }
        public string subject { get; set; }
        public bool TicketSatate { get; set; }
        public long DepartmentId { get; set; }
        #endregion

        #region Realation
        public List<TicketMessage> TicketMessages { get; set; }
        public Department Department { get; set; }
        #endregion
    }
}
