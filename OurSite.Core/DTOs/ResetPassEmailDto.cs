using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs
{
    public class ResetPassEmailDto
    {
        public string ToEmail { get; set; }
        public long Id { get; set; }
        public string UserName { get; set; }
    }
}
