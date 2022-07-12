using System;
namespace OurSite.Core.DTOs.AdminDtos
{
    public class GetAllAdminDto
    {
        public long AdminId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsDelete { get; set; }
    }
}

