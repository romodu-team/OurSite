using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.InvoiceDtos
{
    public class InvoiceDto
    {
        public string InvoiceTitle { get; set; }
        public string UserEmail { get; set; }
        public string InvoicePrice { get; set; }
        public string InvoiceDescription { get; set; }
        public long? ProjectId { get; set; }
    }

    public enum ResInvoice
    {
        Success,
        Failed,
        UserNotFound

    }
}
