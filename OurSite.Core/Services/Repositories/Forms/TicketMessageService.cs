using OurSite.Core.DTOs.TicketsDtos;
using OurSite.Core.Services.Interfaces;
using OurSite.DataLayer.Entities.TicketMessageing;
using OurSite.DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Services.Repositories.Forms
{
    public class TicketMessageService : ITicketMessageService
    {
        #region constructor
        private readonly IGenericReopsitories<TicketMessage> ticketMessageRepo;
        public TicketMessageService(IGenericReopsitories<TicketMessage> ticketMessageRepo)
        {
            this.ticketMessageRepo = ticketMessageRepo;
        }
        #endregion

        #region Send Ticket Message
        public async Task<bool> SendTicketMessage(TicketMessageDto ticketMessageDto)
        {
            TicketMessage messageText = new TicketMessage()
            {
                MessageText = ticketMessageDto.MessageText,
                SubmittedTicketFileName = ticketMessageDto.SubmittedTicketFileName,
                UserOrAdminId=ticketMessageDto.UserOrAdminId,
                IsAdmin=ticketMessageDto.IsAdmin,

            };
            try
            {
                await ticketMessageRepo.AddEntity(messageText);
                await ticketMessageRepo.SaveEntity();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            ticketMessageRepo.Dispose();
        }
        #endregion
    }
}
