using OurSite.Core.DTOs.TicketDtos;
using OurSite.Core.Services.Repositories.TicketServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Services.Interfaces
{
    public interface ITicketService
    {
        Task<bool> CreateTicket();
        Task<ResOperation> UpdateTicket();
        Task<ResOperation> DeleteTicket();
        Task<ResOperation> ChangeTicketStatus();
        Task<ResGetAllTicketDto> GetTicketAssignedToAdmin(long adminId);
        Task<ResGetAllUserTicketsDto> GetUserTickets(long UserId);
        Task<ResGetAllTicketDto> GetAllTickets();
        Task<ResGetTicketDto> GetTicket(long TicketId);
        Task<TicketReportDto> TicketReport();
        Task<bool> CreateDiscussion();
        Task<ResOperation> DeleteDiscussion();
    }
}
