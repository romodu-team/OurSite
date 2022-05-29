using System;
using System.ComponentModel.DataAnnotations;
using OurSite.DataLayer.Entities.BaseEntities;

namespace OurSite.DataLayer.Entities.Accounts
{
	public class User : BaseEntityAccount
	{
        [RegularExpression("0[0-9]{2,}[0-9]{7,}",ErrorMessage ="تلفن ثابت معتبر نیست")]
        public string? Phone { get; set; }
        [RegularExpression("(?:IR)(?=.{24}$)[0-9]",ErrorMessage ="شماره شبا وارد شده معتبر نیست")]
        public string? ShabaNumbers { get; set; }
        public accountType AccountType { get; set; } = accountType.Real;
        [MaxLength(12,ErrorMessage ="کد اقتصادی 12 رقمی است")]
        [MinLength(12, ErrorMessage = "کد اقتصادی 12 رقمی است")]
        public string? BusinessCode { get; set; }
        public string? RegistrationNumber { get; set; }

        public string ActivationCode { get; set; }
        public bool IsActive { get; set; }
    }


    public enum accountType
    {
        Real,
        Legal
    }
}

