using OurSite.DataLayer.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.Payment
{
    public class ZarinPalRequestResponse
    {
        public int Status { get; set; }
        public string Authority { get; set; }
        public string Amount { get; set; }
        public string GateWayUrl { get; set; }
    }
}
