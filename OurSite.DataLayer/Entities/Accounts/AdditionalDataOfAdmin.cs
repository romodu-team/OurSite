using OurSite.DataLayer.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.DataLayer.Entities.Accounts
{
    public class AdditionalDataOfAdmin:BaseEntity
    {
        public long AdminId { get; set; }
        [DisplayName("جنیست")]
        public gender? Gender { get; set; }

        [MaxLength(800, ErrorMessage = " آدرس نمی‌تواند بیش از {0} کاراکتر باشد")]
        [DisplayName("آدرس")]
        public string? Address { get; set; }


      


        [DisplayName("تاریخ تولد")]
        public DateTime? Birthday { get; set; }

    }
    public enum gender
    {
        male,
        female,
        other
    }
}
