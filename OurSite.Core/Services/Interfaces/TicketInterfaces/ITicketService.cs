using OurSite.Core.DTOs.TicketDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Services.Interfaces.TicketInterfaces
{
    public interface ITicketService: IDisposable
    {
        Task<ResCreateDiscussionDto> CreateTicket(ReqCreateTicketDto request,bool IsAdmin);
        Task<ResOperation> UpdateTicket(ReqUpdateTicketDto request);
        Task<ResOperation> DeleteTicket(long TicketId);
        Task<ResOperation> ChangeTicketStatus(long TicketId, long StatusId);
        Task<ResFilteredGetAllTicketDto> GetTicketAssignedToAdmin(long adminId, ReqGetAllTicketDto request);
        Task<ResFilteredGetAllUserTicketDto> GetUserTickets(long UserId, ReqGetAllUserTicketsDto request);
        Task<ResFilteredGetAllTicketDto> GetAllTickets(ReqGetAllTicketDto request);
        Task<ResGetTicketDto> GetTicket(long TicketId);
        Task<TicketReportDto> TicketReport();
        Task<ResCreateDiscussionDto> CreateDiscussion(ReqCreateDiscussion request);
        Task<ResOperation> DeleteDiscussion(List<long> DiscussionsId);
    }
}
