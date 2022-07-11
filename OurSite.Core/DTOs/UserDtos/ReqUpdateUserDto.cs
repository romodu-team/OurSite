using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace OurSite.Core.DTOs.UserDtos
{
	public class ReqUpdateUserDto
	{

        public long id { get; set; }
        public string? FirstName { get; set; }
        public string?LastName { get; set; }
        public string? NationalCode { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? Password { get; set; }
        [Compare("Password" , ErrorMessage = "رمز‌های عبور با یکدیگر مغایرت ندارد")]
        public string? ConfrimPassword { get; set; }
        public gender? Gender { get; set; }
        public string? Address { get; set; }
        public string? ImageName { get; set; }
        public string? Birthday { get; set; }
        public string? Phone { get; set; }
        public string? ShabaNumbers { get; set; }
        public accountType? AccountType { get; set; } = accountType.Real;
        public string? BusinessCode { get; set; }
        public string? RegistrationNumber { get; set; }
        public IFormFile? ProfilePhoto { get; set; }

    }


    public enum accountType
    {
        Real,
        Legal
    }

    public enum gender
    {
        male,
        female,
        other
    }
}


