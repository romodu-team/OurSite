using System;
using OurSite.DataLayer.Entities.Payments;

namespace OurSite.Core.DTOs.Payment
{
    public class EditPayDto
    {
        public long ProId { get; set; }
        public string Titel { get; set; }
        public StatusPay? status { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
    }
}

