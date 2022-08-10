using OurSite.Core.DTOs.TicketDtos;
using OurSite.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Services.Repositories.TicketServices
{
    public class TicketService : ITicketService
    {
        public Task<ResOperation> ChangeTicketStatus()
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateDiscussion()
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateTicket()
        {
            throw new NotImplementedException();
        }

        public Task<ResOperation> DeleteDiscussion()
        {
            throw new NotImplementedException();
        }

        public Task<ResOperation> DeleteTicket()
        {
            throw new NotImplementedException();
        }

        public Task<ResGetAllTicketDto> GetAllTickets()
        {
            throw new NotImplementedException();
        }

        public Task<ResGetTicketDto> GetTicket(long TicketId)
        {
            throw new NotImplementedException();
        }

        public Task<ResGetAllTicketDto> GetTicketAssignedToAdmin(long adminId)
        {
            throw new NotImplementedException();
        }

        public Task<ResGetAllUserTicketsDto> GetUserTickets(long UserId)
        {
            throw new NotImplementedException();
        }

        public Task<TicketReportDto> TicketReport()
        {
            throw new NotImplementedException();
        }

        public Task<ResOperation> UpdateTicket()
        {
            throw new NotImplementedException();
        }
    }
    public enum ResOperation 
    {
        Success,
        Failure,
        NotFound
    }

}
