using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.RoleDtos
{
    public class ReqUpdatePermissionRole
    {
        public long RoleId { get; set; }
        public List<long>? PermissionsId { get; set; }
    }
    public enum resUpdatePermissionRole
    {
        Success,
        RoleNotFound,
        permissionNotFound
        ,
        Error
    }

}
