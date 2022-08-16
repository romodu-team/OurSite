using System;
using System.ComponentModel.DataAnnotations;

namespace OurSite.DataLayer.Entities.BaseEntities
{
	public class BaseEntity
	{
        [Key]
        public long Id { get; set; }
        public Guid UUID { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsRemove { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}

