using OurSite.Core.DTOs.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.TicketDtos
{
    public class ResFilteredGetAllUserTicketDto:BasePaging
    {

        public List<GetAllUserTicketsDto>? Tickets { get; set; }

        public ResFilteredGetAllUserTicketDto SetPaging(BasePaging paging)
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
        public ResFilteredGetAllUserTicketDto SetTickets(List<GetAllUserTicketsDto> tickets)
        {
            this.Tickets = tickets;
            return this;
        }
    }
}
