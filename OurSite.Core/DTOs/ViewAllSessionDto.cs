using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs
{
    public class ViewAllSessionDto
    {
        public long Id { get; set; }
        public string Browser { get; set; }
        public string Platform { get; set; }
        public string LoginDate { get; set; }
        public bool IsCurrentSession { get; set; }
    }
}
