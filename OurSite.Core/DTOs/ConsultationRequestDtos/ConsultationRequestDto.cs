using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.ConsultationRequestDtos
{
    public class ConsultationRequestDto
    {
        [Required(ErrorMessage = "این فیلد اجباری است")]
        [DisplayName("نام و نام خانوادگی")]
        [MaxLength(100, ErrorMessage = "تعداد کاراکتر بیش از حد مجاز است")]
        public string UserFullName { get; set; }

        [EmailAddress(ErrorMessage = "آدرس ایمیل معتبر نیست")]
        [Required(ErrorMessage = "این فیلد اجباری است")]
        [DisplayName("ایمیل")]
        public string UserEmail { get; set; }

        [RegularExpression("09(1[0-9]|3[1-9]|2[1-9])-?[0-9]{3}-?[0-9]{4}", ErrorMessage = "شماره تماس وارد شده صحیح نیست")]
        [Required(ErrorMessage = "این فیلد اجباری است")]
        [DisplayName("شماره همراه")]
        public string UserPhoneNumber { get; set; }

        [Required(ErrorMessage = "لطفا توضیحات را وارد کنید")]
        public string Expration { get; set; }

        [DisplayName("فایل ارسالی")]
        public IFormFile? SubmittedFile { get; set; }

        public List<string>? ItemSelectedsId { get; set; }
    }
}
