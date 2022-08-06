using Microsoft.EntityFrameworkCore;
using OurSite.Core.DTOs.Paging;
using OurSite.Core.DTOs.TicketsDtos;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Utilities;
using OurSite.Core.Utilities.Extentions.Paging;
using OurSite.DataLayer.Contexts;
using OurSite.DataLayer.Entities.Ticketing;
using OurSite.DataLayer.Entities.TicketMessageing;
using OurSite.DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Services.Repositories.Forms
{
    public class TicketService : ITicketService
    {

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

        #region List Of All Tickets
        public async Task<ResFilterTicketDto> GetAllTicket(ReqFilterTicketDto filter)
        {
            var ticketQuery = ticketRepo.GetAllEntity();
            var count = (int)Math.Ceiling(ticketQuery.Count() / (double)filter.TakeEntity);
            var pager = Pager.Build(count, filter.PageId, filter.TakeEntity);
            var list = await ticketRepo.GetAllEntity().Paging(pager).Where(u => u.IsRemove == false).Select(x => new GetAllTicketDto { TicketId = x.Id, TicketTitle = x.TicketTitle,TicketSubject=x.TicketSubject,DepartmentName=x.Department.DepartmentTitle, IsClosed = x.IsClosed }).ToListAsync();    //use the genric interface options and save values in variable
            var result = new ResFilterTicketDto();
            result.SetPaging(pager);
            return result.SetTickets(list);
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

        #region Change Ticket Status
        public async Task<bool> ChangeTicketStatus(long ticketId)
        {
            try
            {
                var ticket = await ticketRepo.GetEntity(ticketId);
                ticket.IsClosed = !ticket.IsClosed;
                ticketRepo.UpDateEntity(ticket);
                await ticketRepo.SaveEntity();
                return true;

            }
            catch (Exception)
            {
                return false;
            }

        }

        #endregion

        #region Get All User Ticket
        public async Task<List<GetAllTicketDto>> GetAllUserTicket(long userId)
        {
            var user = ticketRepo.GetAllEntity().Where(x => x.UserId == userId).Include(x => x.Department).Select(x => new GetAllTicketDto { TicketTitle = x.TicketTitle, DepartmentName = x.Department.DepartmentName, IsClosed = x.IsClosed, TicketId = x.Id, TicketSubject = x.TicketSubject });
            return await user.ToListAsync();
        }
        #endregion
    }
}