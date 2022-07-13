using OurSite.DataLayer.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.DataLayer.Entities.Accounts
{
    public class AdditionalDataOfUser:BaseEntity
    {
        public long UserId { get; set; }
        [RegularExpression("0[0-9]{2,}[0-9]{7,}", ErrorMessage = "تلفن ثابت معتبر نیست")]
        public string? Phone { get; set; }
        [RegularExpression("(?:IR)(?=.{24}$)[0-9]", ErrorMessage = "شماره شبا وارد شده معتبر نیست")]
        public string? ShabaNumbers { get; set; }
        [MaxLength(12, ErrorMessage = "کد اقتصادی 12 رقمی است")]
        [MinLength(12, ErrorMessage = "کد اقتصادی 12 رقمی است")]
        public string? BusinessCode { get; set; }
        public string? RegistrationNumber { get; set; }

        [DisplayName("جنیست")]
        public gender? Gender { get; set; }

        [MaxLength(800, ErrorMessage = " آدرس نمی‌تواند بیش از {0} کاراکتر باشد")]
        [DisplayName("آدرس")]
        public string? Address { get; set; }


        [DisplayName("عکس کاربری")]
        public string? ImageName { get; set; }


        [DisplayName("تاریخ تولد")]
        public string? Birthday { get; set; }

    }
}
