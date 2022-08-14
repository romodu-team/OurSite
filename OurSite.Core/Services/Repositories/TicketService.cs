using OurSite.Core.DTOs;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Utilities;
using OurSite.DataLayer.Entities.Ticketing;
using OurSite.DataLayer.Entities.TicketMessageing;
using OurSite.DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Services.Repositories
{
    public class TicketService : ITicketService
    {
        #region constructor
        private readonly IGenericReopsitories<Ticket> ticketRepo;
        private readonly IGenericReopsitories<TicketMessage> ticketMessageRepo;
        public TicketService(IGenericReopsitories<Ticket> ticketRepo , IGenericReopsitories<TicketMessage> ticketMessageRepo)
        {
            this.ticketRepo = ticketRepo;
            this.ticketMessageRepo = ticketMessageRepo;
        }
        #endregion


        public async Task<ResTicketDto> createTicket(TicketDto ticketDto)
        {
            Ticket createTicket = new Ticket()
            {
                TicketTitle = ticketDto.TicketTitle,
                TicketSubject = ticketDto.TicketSubject,
                CreateDate = DateTime.Now,
                LastUpdate = DateTime.Now,
                DepartmentId = ticketDto.DepartmentId,
                IsClosed = false,
                UserId = ticketDto.UserId

            };
            TicketMessage message = new TicketMessage()
            {
                MessageText = ticketDto.MessageText,
                UserOrAdminId = ticketDto.UserId,
                CreateDate = DateTime.Now,
                LastUpdate = DateTime.Now,
                IsSeen = false,

            };
            if (ticketDto.SubmittedTicketFileName != null)
            {
                message.SubmittedTicketFileName = ticketDto.SubmittedTicketFileName;
            }
            try
            {
                await ticketRepo.AddEntity(createTicket);
                await ticketRepo.SaveEntity();
                message.TicketId = createTicket.Id;
                await ticketMessageRepo.AddEntity(message);
                await ticketRepo.SaveEntity();
                return new ResTicketDto {resTicket= ResTicket.Success};
            }
            catch
            {
               return new ResTicketDto {resTicket= ResTicket.Failed};
            }
        }

        #region Dispose
        public void Dispose()
        {
            ticketRepo.Dispose();
            ticketMessageRepo.Dispose();

        }
        #endregion
    }
}
