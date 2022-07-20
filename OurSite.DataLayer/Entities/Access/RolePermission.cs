using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OurSite.DataLayer.Entities.BaseEntities;

namespace OurSite.DataLayer.Entities.Access
{
	public class RolePermission : BaseEntity
    {
        public long RolesId { get; set; }
        public long PermissionId { get; set; }
        
        [ForeignKey("RolesId")]
        public Role? Roles { get; set; }
        public Permission Permission { get; set; }
    }
}

