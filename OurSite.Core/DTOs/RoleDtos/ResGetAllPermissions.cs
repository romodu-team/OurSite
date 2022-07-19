using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.RoleDtos
{
    public class ResGetAllPermissions
    {
        public long PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string PermissionTitle { get; set; }
        public long? ParentId { get; set; }
        public bool IsCheck { get; set; }
    }
}
