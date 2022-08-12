using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.TicketDtos
{
    public class TicketCategoryDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public long? ParentId { get; set; }
    }
}
