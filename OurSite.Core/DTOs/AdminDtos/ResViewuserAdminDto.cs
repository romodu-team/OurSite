using System;
using OurSite.Core.DTOs.UserDtos;
using OurSite.DataLayer.Entities.Accounts;
using gender = OurSite.DataLayer.Entities.Accounts.gender;

namespace OurSite.Core.DTOs.AdminDtos
{
    public class ResViewuserAdminDto
    {
        public long Id { get; set; }
        public Guid UserUUID{ get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsRemove { get; set; }
        public DateTime LastUpdate { get; set; }
        public string UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? NationalCode { get; set; }
        public string? Email { get; set; }
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

    }
}


