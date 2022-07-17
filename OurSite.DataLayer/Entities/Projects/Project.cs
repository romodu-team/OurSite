using System;
using OurSite.DataLayer.Entities.Accounts;
using OurSite.DataLayer.Entities.BaseEntities;

namespace OurSite.DataLayer.Entities.Projects
{
    public class Project : BaseEntity
    {
        public string Name { get; set; }
        public ProType Type { get; set; }
        public  DateTime dateTime { get; set; }
        public DateTime EndTime { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public situations Situation { get; set; }
        public string PlanDetails { get; set; }
        public long AdminId { get; set; }
        public long UserId { get; set; }
        public string ContractFileName { get; set; }



        #region Relations
        public Admin Admin { get; set; }
        public User User { get; set; }
        #endregion
    }

    public enum ProType
    {
        WebDesign,
        Cms,
        Graphic,
        Appliction,
        seo
    }

    public enum situations
    {
        Cancel,
        Doing,
        AwatingPayment,
        Pending,
        End
    }

}

