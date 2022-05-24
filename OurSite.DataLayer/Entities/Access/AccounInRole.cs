using System;
using OurSite.DataLayer.Entities.Accounts;
using OurSite.DataLayer.Entities.BaseEntities;

namespace OurSite.DataLayer.Entities.Access
{
	public class AccounInRole : BaseEntity
	{

        #region Properties
        public long AdminId { get; set; }
        public long RoleId { get; set; }
        #endregion


        #region Relation
        public Admin Admin { get; set; }
        public Role Role { get; set; }
        #endregion

    }
}

