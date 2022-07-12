using Microsoft.EntityFrameworkCore;
using OurSite.Core.DTOs.TicketsDtos;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Utilities;
using OurSite.DataLayer.Contexts;
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
        private DataBaseContext context;

        #region constructor
        private readonly IGenericReopsitories<Ticket> ticketRepo;
        private readonly IGenericReopsitories<TicketMessage> ticketMessageRepo;
        public TicketService(IGenericReopsitories<Ticket> ticketRepo, IGenericReopsitories<TicketMessage> ticketMessageRepo)
        {
            this.ticketRepo = ticketRepo;
            this.ticketMessageRepo = ticketMessageRepo;
        }
        #endregion

        #region Create Ticket
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
                return new ResTicketDto { resTicket = ResTicket.Success };
            }
            catch
            {
                return new ResTicketDto { resTicket = ResTicket.Failed };
            }
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            ticketRepo.Dispose();
            ticketMessageRepo.Dispose();

        }
        #endregion

        #region Ticket List
        public async Task<List<GetAllTicketDto>> GetAllTicket()
        {
            var list = await ticketRepo.GetAllEntity().Where(x => x.IsRemove == false).Select(x => new GetAllTicketDto { TicketId = x.Id, TicketTitle = x.TicketTitle, IsClosed = x.IsClosed }).ToListAsync();
            return list;
        }

        #endregion

        #region Find By Id
        public async Task<ResViewTicketDto> FindTicketById(long ticketId)
        {
            var ticket = await ticketRepo.GetEntity(ticketId);

            ResViewTicketDto res = new ResViewTicketDto
            {
                TicketId = ticket.Id,
                TicketTitle = ticket.TicketTitle,
                DepartmentId = ticket.DepartmentId,
                IsClosed = ticket.IsClosed,
                TicketSubject = ticket.TicketSubject

            };

            return res;
        }
        #endregion

        #region Delete Ticket
        public async Task<bool> DeleteTicket(long TicketId)
        {
            var isdelete = await ticketRepo.DeleteEntity(TicketId);
            if (isdelete)
            {
                try
                {
                    await ticketRepo.SaveEntity();

                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return isdelete;

        }
        #endregion

    }
}
