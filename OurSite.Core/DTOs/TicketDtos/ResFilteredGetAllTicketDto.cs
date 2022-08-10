using OurSite.Core.DTOs.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.TicketDtos
{
    public class ResFilteredGetAllTicketDto: BasePaging
    {
        public List<GetAllTicketDto>? Tickets { get; set; }

        public ResFilteredGetAllTicketDto SetPaging(BasePaging paging)
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
        public ResFilteredGetAllTicketDto SetTickets(List<GetAllTicketDto> tickets)
        {
            this.Tickets = tickets;
            return this;
        }
    }
}
