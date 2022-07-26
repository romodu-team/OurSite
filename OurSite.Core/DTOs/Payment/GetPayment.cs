using System;
using OurSite.DataLayer.Entities.Payments;

namespace OurSite.Core.DTOs.Payment
{
    public class GetPayment
    {
        public string Titel { get; set; }
        public StatusPay status { get; set; }
        public long UserId { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
        public long ProId { get; set; }
        public DateTime DatePay { get; set; }
    }
}

