using OurSite.Core.DTOs.TicketsDtos;
using OurSite.DataLayer.Entities.Ticketing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Services.Interfaces
{
    public interface ITicketService : IDisposable
    {
        Task<ResTicketDto> createTicket(TicketDto ticketDto);
        Task<List<GetAllTicketDto>> GetAllTicket();
        Task<ResViewTicketDto> FindTicketById(long ticketId);
        Task<bool> DeleteTicket(long ticketId);

    }
}
