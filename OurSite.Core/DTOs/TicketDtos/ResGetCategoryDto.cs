using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.TicketDtos
{
    public class ResGetCategoryDto
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public string CreateDate { get; set; }
        public string LastUpdateDate { get; set; }
        public long Id { get; set; }

    }
}
