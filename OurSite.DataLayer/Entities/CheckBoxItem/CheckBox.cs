using OurSite.DataLayer.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.DataLayer.Entities.CheckBoxItem
{
    public class CheckBox : BaseEntity
    {
        public string Title { get; set; }
        public bool IsChecked { get; set; }
    }
}
