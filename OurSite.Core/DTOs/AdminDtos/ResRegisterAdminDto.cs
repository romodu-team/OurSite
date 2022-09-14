using OurSite.Core.DTOs.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.AdminDtos
{
    public class ResRegisterAdminDto
    {
        public RessingupDto RessingupDto { get; set; }
        public long? AdminId { get; set; }
        public Guid? AdminUUID { get; set; }
    }
}
