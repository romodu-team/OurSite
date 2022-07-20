using System;
using OurSite.DataLayer.Entities.Access;
using OurSite.DataLayer.Entities.BaseEntities;
using OurSite.DataLayer.Entities.Projects;

namespace OurSite.DataLayer.Entities.Accounts
{
	public class Admin : BaseEntityAccount
	{

        #region Relations
        public ICollection<AccounInRole> AccounInRoles { get; set; }

        public AdditionalDataOfAdmin? additionalDataOfAdmin { get; set; }

        public ICollection<Project> Projects { get; set; }


        #endregion

    }
}

