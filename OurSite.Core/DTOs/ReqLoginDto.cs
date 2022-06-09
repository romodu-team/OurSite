using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs
{
    public class ReqLoginDto
    {
        [Required(ErrorMessage ="نام کاربری یا ایمیل اجباری است")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "رمز عبور اجباری است")]
        public string Password { get; set; }
    }

    public enum ResLoginDto
    {
        Success,
        IncorrectData,
        NotActived,
        Error
    }
}
