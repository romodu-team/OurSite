using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using OurSite.DataLayer.Entities.BaseEntities;

namespace OurSite.DataLayer.Entities.Access
{
	public class Permission : BaseEntity
    {
        public string PermissionTitle{get;set;}
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
