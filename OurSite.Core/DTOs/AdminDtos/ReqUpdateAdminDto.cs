using OurSite.Core.DTOs.UserDtos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.AdminDtos
{
    public class ReqUpdateAdminDto
    {
        public long  adminId { get; set; }
       // [Required(ErrorMessage = "این فیلد اجباری است")]
        [DisplayName("نام")]
        [MaxLength(50, ErrorMessage = "تعداد کاراکتر بیش از حد مجاز است")]
        public string? FirstName { get; set; }

      //  [Required(ErrorMessage = "این فیلد اجباری است")]
        [DisplayName("نام خانوادگی")]
        [MaxLength(50, ErrorMessage = "تعداد کاراکتر بیش از حد مجاز است")]
        public string? LastName { get; set; }

        [MinLength(10), MaxLength(10)]
        [DisplayName("کد ملی")]
      //  [Required(ErrorMessage = "این فیلد اجباری است")]
        public string? NationalCode { get; set; }

        [EmailAddress(ErrorMessage = "آدرس ایمیل معتبر نیست")]
      //  [Required(ErrorMessage = "این فیلد اجباری است")]
        [DisplayName("ایمیل")]
        public string? Email { get; set; }

        [RegularExpression("09(1[0-9]|3[1-9]|2[1-9])-?[0-9]{3}-?[0-9]{4}", ErrorMessage = "شماره تماس وارد شده صحیح نیست")]
    //    [Required(ErrorMessage = "این فیلد اجباری است")]
        [DisplayName("شماره همراه")]
        public string? Mobile { get; set; }

        //[Required(ErrorMessage = "این فیلد اجباری است")]
        [MinLength(3, ErrorMessage = "نام کاربری باید حداقل {0} کاراکتر باید باشد")]
        [DisplayName("نام کاربری")]
        public string? UserName { get; set; }

        [DisplayName("جنیست")]
        public gender? Gender { get; set; }

        [MaxLength(800, ErrorMessage = " آدرس نمی‌تواند بیش از {0} کاراکتر باشد")]
        [DisplayName("آدرس")]
        public string? Address { get; set; }


        [DisplayName("عکس کاربری")]
        public string? ImageName { get; set; }


        [DisplayName("تاریخ تولد")]
        public string? Birthday { get; set; }

        public string? RoleName { get; set; }
    }
    public enum resUpdateAdmin
    {
        Success,
        NotFound,
        Error
    }
}
