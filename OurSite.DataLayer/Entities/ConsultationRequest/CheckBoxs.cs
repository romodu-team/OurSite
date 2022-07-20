using OurSite.DataLayer.Entities.BaseEntities;
using OurSite.DataLayer.Entities.Projects;
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

        #region Realation
        public ICollection<ItemSelected> ItemSelecteds { get; set; }
        public ICollection<SelectedProjectPlan> selectedProjectPlans{get;set;}
        #endregion
    }
    public enum section
    {
        ConsultationForm,
        PlanProject
    }
}
