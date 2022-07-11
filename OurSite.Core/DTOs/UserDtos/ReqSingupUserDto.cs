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
        public string Mobile { get; set; }
        public string Email { get; set; }
     //   public singup Singup { get; set; }
        public long? AdminRoleId { get; set; }
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

