using OurSite.Core.DTOs.TicketsDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Services.Interfaces
{
    public interface ITicketMessageService : IDisposable
    {
        Task<bool> SendTicketMessage(TicketMessageDto ticketMessageDto);
    }
}
