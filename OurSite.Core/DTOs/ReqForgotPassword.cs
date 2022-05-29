using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs
{
    public class ReqForgotPassword
    {
        public long UserId { get; set; }
        public string Password { get; set; }

        [Compare("Password",ErrorMessage ="رمز های عبور وارد شده مغایرت دارند")]
        public string RePassword { get; set; }
    }
}
