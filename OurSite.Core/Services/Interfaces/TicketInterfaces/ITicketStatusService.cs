using OurSite.DataLayer.Entities.Ticketing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Services.Interfaces.TicketInterfaces
{
    public interface ITicketStatusService
    {
        Task<bool> CreateStatus(string title, string name);
        Task<bool> DeleteStatus(long StatusId);
        Task<TicketStatus> GetStatus(long StatusId);
        Task<bool> UpdateStatus(long StatusId, string? title, string? name);
        Task<List<TicketStatus>> GetAllStatus();
    }
}
