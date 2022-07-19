using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OurSite.DataLayer.Entities.BaseEntities;

namespace OurSite.DataLayer.Entities.Access
{
	public class Permission : BaseEntity
    {
        public string PermissionTitle{get;set;}
        public string PermissionName{get;set;}
        public long? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public Permission? Parent { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
