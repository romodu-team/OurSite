using OurSite.Core.DTOs;
using OurSite.Core.Services.Interfaces;
using OurSite.DataLayer.Entities.Ticketing;
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
        public TicketService(IGenericReopsitories<Ticket> ticketRepo)
        {
            this.ticketRepo = ticketRepo;
        }
        #endregion


        public async Task<bool> createTicket(TicketDto ticketDto)
        {
            Ticket createTicket = new Ticket()
            {
                TicketTitle = ticketDto.TicketTitle,
                TicketSubject = ticketDto.TicketSubject,
            };
            try
            {
                await ticketRepo.AddEntity(createTicket);
                await ticketRepo.SaveEntity();
                return true;
            }
            catch
            {
                return false;
            }
        }

        #region Dispose
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
