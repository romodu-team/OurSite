using System;
using System.ComponentModel.DataAnnotations;
using OurSite.DataLayer.Entities.BaseEntities;

namespace OurSite.DataLayer.Entities.Accounts
{
	public class User : BaseEntityAccount
	{
       
        public accountType AccountType { get; set; } = accountType.Real;
       
        public string? ActivationCode { get; set; }

        #region Realations
        public AdditionalDataOfUser? AdditionalDataOfUser { get; set; }
        #endregion
    }


    public enum accountType
    {
        Real,
        Legal
    }
}

