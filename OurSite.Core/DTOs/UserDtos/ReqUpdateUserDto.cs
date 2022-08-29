using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using OurSite.DataLayer.Entities.Accounts;

namespace OurSite.Core.DTOs.UserDtos
{
	public class ReqUpdateUserDto
	{

        public string? FirstName { get; set; }
        public string?LastName { get; set; }
        public string? NationalCode { get; set; }
        public string? Email { get; set; }
        [RegularExpression("09(1[0-9]|3[1-9]|2[0-9]|9[0-9]|0[1-9]|4[1-9])-?[0-9]{3}-?[0-9]{4}",ErrorMessage = "شماره تماس وارد شده صحیح نیست")]

        public string? Mobile { get; set; }
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


    //public enum accountType
    //{
    //    Real,
    //    Legal
    //}

    public enum gender
    {
        male,
        female,
        other
    }


    public enum ResUpdateProfile
    {
        Success,
        Failed,
        
    }
}


