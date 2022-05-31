using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs
{
    public class SendEmailDto
    {
        public string ToEmail { get; set; }
        public string Parameter { get; set; }
        public string UserName { get; set; }
    }
}
