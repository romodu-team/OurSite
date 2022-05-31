using System;
using System.ComponentModel.DataAnnotations;

namespace OurSite.Core.DTOs
{
	public class ReqSingupUserDto
	{
        public string Name { get; set; }
        public string Family { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        [Compare("Password" , ErrorMessage = "رمز‌های عبور با یکدیگر مغایرت ندارند")]
        public string ConmfrimPassword { get; set; }
        public string phone { get; set; }
        public string Email { get; set; }
        public singup Singup { get; set; }

    }

	public enum singup
    {
		success,
		Failed,
		Exist
    };

    public enum ResActiveUser
    {
        Success,
        Failed
    }
}

