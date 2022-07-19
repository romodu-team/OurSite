using OurSite.DataLayer.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.Payment
{
    public class ZarinPalVerifyResponse
    {
        public int Status { get; set; }
        public string RefID { get; set; }
    }
}
