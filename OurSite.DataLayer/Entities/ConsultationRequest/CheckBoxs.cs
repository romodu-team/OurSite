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
        #region Properties
        public string CheckBoxName { get; set; }
        public section sectionName { get; set; }
        #endregion


        #region Realation
        public ICollection<ItemSelected> ItemSelecteds { get; set; }
        #endregion
    }
    public enum section
    {
        ConsultationForm,
        PlanProject
    }
}
