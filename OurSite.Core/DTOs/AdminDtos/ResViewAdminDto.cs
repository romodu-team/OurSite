using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.AdminDtos
{
    public class ResViewAdminDto
    {
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsRemove { get; set; }
        public DateTime LastUpdate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; }
        public string? Address { get; set; }
        public string? ImageName { get; set; }
        public string? Birthday { get; set; }

        public string RoleName { get; set; }

    }
}
