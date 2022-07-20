using OurSite.DataLayer.Entities.Accounts;
using OurSite.DataLayer.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OurSite.DataLayer.Entities.Projects;

namespace OurSite.DataLayer.Entities.Factor
{
    public class Invoice : BaseEntity
    {
        #region Properties
        public string Title { get; set; }
        public long? UserId { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
        public long? ProjectId { get; set; }
        #endregion

        #region Realation

        [ForeignKey("UserId")]
        public User User { get; set; }
        public Project Project { get; set; }

        #endregion
    }
}
