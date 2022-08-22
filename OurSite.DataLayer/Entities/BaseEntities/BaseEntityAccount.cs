using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OurSite.DataLayer.Entities.BaseEntities
{
	public class BaseEntityAccount:BaseEntity
	{

        [DisplayName("نام")]
        [MaxLength(50,ErrorMessage ="تعداد کاراکتر بیش از حد مجاز است")]
        public string? FirstName { get; set; }

        [DisplayName("عکس کاربری")]
        public string? ImageName { get; set; }

        [DisplayName("نام خانوادگی")]
        [MaxLength(50, ErrorMessage = "تعداد کاراکتر بیش از حد مجاز است")]
        public string? LastName { get; set; }



        [MinLength(10) , MaxLength(10)]
        [DisplayName("کد ملی")]
        public string? NationalCode { get; set; }

        [EmailAddress(ErrorMessage = "آدرس ایمیل معتبر نیست")]
        [DisplayName("ایمیل")]
        public string? Email { get; set; }

        [RegularExpression("09(1[0-9]|3[1-9]|2[0-9]|9[0-9]|0[1-9]|4[1-9])-?[0-9]{3}-?[0-9]{4}",ErrorMessage = "شماره تماس وارد شده صحیح نیست")]
        [DisplayName("شماره همراه")]
        public string? Mobile { get; set; }


        [Required(ErrorMessage = "این فیلد اجباری است")]
        [MinLength(8 , ErrorMessage = "رمز عبور باید حداقل 8 کاراکتر باشد")]
        [DisplayName("رمز عبور")]
        public string Password { get; set; }

        [Required(ErrorMessage = "این فیلد اجباری است")]
        [MinLength(3, ErrorMessage = "نام کاربری باید حداقل {0} کاراکتر باید باشد")]
        [DisplayName("نام کاربری")]
        public string UserName { get; set; }

        public bool IsActive { get; set; }

    }



}

