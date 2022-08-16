using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.RoleDtos
{
    public class RoleDto
    {
        public long RoleId { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }


    }
    public enum ResAddRole
    {
        Success,
        Faild,
        Exist,
        InvalidInput
    }
    public enum ResDeleteAdminRole
    {
        Success,
        Faild,
        NotExist
    }
}

