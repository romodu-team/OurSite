using OurSite.DataLayer.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.DataLayer.Entities.ConsultationRequest
{
    public class ItemSelected : BaseEntity
    {
        public long ConsultationFormId { get; set; }
        public long CheckBoxId { get; set; }

        #region Realation
        [ForeignKey("ConsultationFormId")]
        public ConsultationRequest ConsultationRequests { get; set; }
        [ForeignKey("CheckBoxId")]

        public CheckBoxs CheckBox { get; set; }

        #endregion
    }
}
