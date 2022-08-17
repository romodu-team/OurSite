using System;
using System.ComponentModel.DataAnnotations;

namespace OurSite.Core.DTOs.AdminDtos
{
    public class ReqRegisterAdminDto
    {
        public string Name { get; set; }
        public string Family { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "رمز‌های عبور با یکدیگر مغایرت ندارند")]
        public string ConmfrimPassword { get; set; }
        [RegularExpression("09(1[0-9]|3[1-9]|2[0-9]|9[0-9]|0[1-9]|4[1-9])-?[0-9]{3}-?[0-9]{4}",ErrorMessage = "شماره تماس وارد شده صحیح نیست")]
        public string Mobile { get; set; }
        public string Email { get; set; }
        //   public singup Singup { get; set; }
        public long? AdminRoleId { get; set; }
    }
    
}

