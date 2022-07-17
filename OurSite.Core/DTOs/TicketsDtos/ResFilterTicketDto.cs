using OurSite.Core.DTOs.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.TicketsDtos
{
    public class ResFilterTicketDto : BasePaging
    {
        public List<GetAllTicketDto>? Tickets { get; set; }

        public ResFilterTicketDto SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.PageCount = paging.PageCount;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.TakeEntity = paging.TakeEntity;
            this.SkipEntity = paging.SkipEntity;
            this.ActivePage = paging.ActivePage;
            return this;
        }
        public ResFilterTicketDto SetTickets(List<GetAllTicketDto> tickets)
        {
            this.Tickets = tickets;
            return this;
        }
    }
}
