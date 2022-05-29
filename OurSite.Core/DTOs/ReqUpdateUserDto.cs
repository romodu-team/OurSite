using System;
namespace OurSite.Core.DTOs
{
	public class ReqUpdateUserDto
	{

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public gender Gender { get; set; }
        public string Address { get; set; }
        public string ImageName { get; set; }
        public string Birthday { get; set; }
        public string Phone { get; set; }
        public string? ShabaNumbers { get; set; }
        public accountType AccountType { get; set; } = accountType.Real;
        public string? BusinessCode { get; set; }
        public string? RegistrationNumber { get; set; }


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


