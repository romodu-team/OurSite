using System;
using OurSite.DataLayer.Entities.Access;
using OurSite.DataLayer.Entities.BaseEntities;

namespace OurSite.DataLayer.Entities.Accounts
{
	public class Admin : BaseEntityAccount
	{
        public bool IsActive { get; set; } = true;
        #region Relations
        public ICollection<AccounInRole> AccounInRoles { get; set; }
        #endregion

    }
}

