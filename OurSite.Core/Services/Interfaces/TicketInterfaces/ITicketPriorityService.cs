using OurSite.Core.DTOs.TicketDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Services.Interfaces.TicketInterfaces
{
    public interface ITicketPriorityService: IDisposable
    {
        Task<bool> CreatePriority(string title, string name);
        Task<ResDeleteOpration> DeletePriority(long priorityId);
        Task<TicketPriorityDto> GetPriority(long priorityId);
        Task<bool> UpdatePriority(long priorityId, string? title, string? name);
        Task<List<TicketPriorityDto>> GetAllPriority();

    }
}
