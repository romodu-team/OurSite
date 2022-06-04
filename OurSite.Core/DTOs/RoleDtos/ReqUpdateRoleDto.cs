using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.RoleDtos
{
    public class ReqUpdateRoleDto
    {
        public long RoleID { get; set; }
        public string RoleTitle { get; set; }

        public enum ResUpdateRole
        {
            Success,
            Error,
            NotFound
        }
    }
}
