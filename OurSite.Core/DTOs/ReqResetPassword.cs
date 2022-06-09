using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs
{
    public class ReqResetPassword
    {
        [Required(ErrorMessage ="فیلد id اجباری است")]
        public long UserId { get; set; }
        [Required(ErrorMessage = "رمز عبور اجباری است")]
        public string Password { get; set; }
        [Required(ErrorMessage ="تکرار رمز عبور اجباری است")]

        [Compare("Password",ErrorMessage ="رمز های عبور وارد شده مغایرت دارند")]
        public string RePassword { get; set; }
    }
}
