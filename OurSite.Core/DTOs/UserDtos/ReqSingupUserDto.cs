using System;
using System.ComponentModel.DataAnnotations;
using OurSite.DataLayer.Entities.Accounts;

namespace OurSite.Core.DTOs.UserDtos
{
	public class ReqSingupUserDto
	{
        public string Name { get; set; }
        public string Family { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        [Compare("Password" , ErrorMessage = "رمز‌های عبور با یکدیگر مغایرت ندارند")]
        public string ConmfrimPassword { get; set; }
        [RegularExpression("09(1[0-9]|3[1-9]|2[0-9]|9[0-9]|0[1-9]|4[1-9])-?[0-9]{3}-?[0-9]{4}",ErrorMessage = "شماره تماس وارد شده صحیح نیست")]

        public string Mobile { get; set; }
        public string Email { get; set; }
     //   public singup Singup { get; set; }
        public accountType AccountType { get; set; }
    }

    //public enum accountType
    //{
    //    Real,
    //    Legal
    //}

    public enum RessingupDto
    {
		success,
		Failed,
		Exist,
        MobileExist
    };

    public enum ResActiveUser
    {
        Success,
        Failed,
        NotFoundOrActivated
    }
}

