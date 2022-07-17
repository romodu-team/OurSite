using OurSite.DataLayer.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.DataLayer.Entities.ConsultationRequest
{
    public class CheckBoxs : BaseEntity
    {
        public string CheckBoxName { get; set; }
        public section sectionName { get; set; }

        public ICollection<ItemSelected> ItemSelecteds { get; set; }
    }
    public enum section
    {
        ConsultationForm,
        PlanProject
    }
}
