using System;
using OurSite.DataLayer.Entities.Accounts;
using OurSite.DataLayer.Entities.BaseEntities;
using OurSite.DataLayer.Entities.Projects;

namespace OurSite.DataLayer.Entities.Payments
{
    public class Payment : BaseEntity
    {
        #region Property
        public string Titel { get; set; }
        public StatusPay status { get; set; }
        public long UserId { get; set; }
        public long AdminId { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
        public long ProId { get; set; }
        public DateTime? DatePay { get; set; }
        #endregion


        #region Realtions
        public Project Project { get; set; }
        public User User { get; set; }
        #endregion



    }


    public enum StatusPay
    {
        Success,
        Faild,
        Pending

    }
}

