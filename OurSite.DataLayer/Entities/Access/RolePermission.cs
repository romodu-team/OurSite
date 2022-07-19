using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using OurSite.DataLayer.Entities.BaseEntities;

namespace OurSite.DataLayer.Entities.Access
{
	public class RolePermission : BaseEntity
    {
        public long RoleId { get; set; }
        public long PermissionId { get; set; }
        
        public Role Role { get; set; }
        public Permission Permission { get; set; }
    }
}

