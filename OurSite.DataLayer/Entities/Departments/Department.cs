using OurSite.DataLayer.Entities.BaseEntities;
using OurSite.DataLayer.Entities.Ticketing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.DataLayer.Entities.Departments
{
    public class Department : BaseEntity
    {
        #region Properties
        public string DepartmentName { get; set; }
        public string DepartmentTitle { get; set; }
        #endregion

        #region Realation
        public ICollection<Ticket> Ticket { get; set; }
        #endregion 
    }
}
