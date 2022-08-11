using OurSite.DataLayer.Entities.Ticketing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Services.Interfaces.TicketInterfaces
{
    public interface ITicketPriorityService
    {
        Task<bool> CreatePriority(string title, string name);
        Task<bool> DeletePriority(long priorityId);
        Task<TicketPriority> GetPriority(long priorityId);
        Task<bool> UpdatePriority(long priorityId, string? title, string? name);
        Task<List<TicketPriority>> GetAllPriority();

    }
}
