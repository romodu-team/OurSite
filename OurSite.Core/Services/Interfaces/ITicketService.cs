using OurSite.Core.DTOs.TicketDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Services.Interfaces
{
    public interface ITicketService
    {
        Task<ResOperation> CreateTicket(ReqCreateTicketDto request);
        Task<ResOperation> UpdateTicket();
        Task<ResOperation> DeleteTicket();
        Task<ResOperation> ChangeTicketStatus(long TicketId,long StatusId);
        Task<ResGetAllTicketDto> GetTicketAssignedToAdmin(long adminId);
        Task<ResGetAllUserTicketsDto> GetUserTickets(long UserId);
        Task<ResGetAllTicketDto> GetAllTickets();
        Task<ResGetTicketDto> GetTicket(long TicketId);
        Task<TicketReportDto> TicketReport();
        Task<bool> CreateDiscussion(ReqCreateDiscussion request);
        Task<ResOperation> DeleteDiscussion();
    }
}
