using System;
using OurSite.DataLayer.Entities.Payments;
using OurSite.DataLayer.Entities.Projects;

namespace OurSite.Core.DTOs.Payment
{
    public class GetAllPayDto
    {
        public long PayId { get; set; }
        public StatusPay status { get; set; }
        public string Username { get; set; }
        public string Price { get; set; }
        public long ProId { get; set; }
        public DateTime? DatePay { get; set; }
    }
}

