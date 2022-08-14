using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.Payment
{
    public class PaymentRequestDto
    {
        public string amount { get; set; }
        public string phoneNumber { get; set; }
        public string description { get; set; }
    }
}
