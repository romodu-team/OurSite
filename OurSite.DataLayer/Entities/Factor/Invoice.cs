using OurSite.DataLayer.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.DataLayer.Entities.Factor
{
    public class Invoice : BaseEntity
    {
        public string Title { get; set; }
        public string UserEmail { get; set; }
        public long? UserId { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
        public bool IsRemove { get; set; }
    }
}
