using System;
using OurSite.DataLayer.Entities.Access;
using OurSite.DataLayer.Entities.BaseEntities;

namespace OurSite.DataLayer.Entities.Accounts
{
	public class Admin : BaseEntityAccount
	{
        #region Relations
        public ICollection<AccounInRole> AccounInRoles { get; set; }

        public AdditionalDataOfAdmin? additionalDataOfAdmin { get; set; }

        #endregion

    }
}

