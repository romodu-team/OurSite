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


        //public datareturned data { get; set; }
        //public List<string> errors { get; set; }
    }
    //public class datareturned
    //{
    //    public string code { get; set; }
    //    public string message { get; set; }
    //    public string card_hash { get; set; }
    //    public string card_pan { get; set; }
    //    public string ref_id { get; set; }
    //    public string fee_type { get; set; }
    //    public string fee { get; set; }
    //}
}
