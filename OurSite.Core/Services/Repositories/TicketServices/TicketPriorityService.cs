using OurSite.Core.Services.Interfaces.TicketInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OurSite.DataLayer.Interfaces;
using OurSite.DataLayer.Entities.Ticketing;
using Microsoft.EntityFrameworkCore;

namespace OurSite.Core.Services.Repositories.TicketServices
{
    public class TicketPriorityService : ITicketPriorityService
    {
        private IGenericReopsitories<TicketPriority> _priorityRepository;
        public TicketPriorityService(IGenericReopsitories<TicketPriority> PriorityRepository)
        {
            _priorityRepository = PriorityRepository;
        }
        public async Task<bool> CreatePriority(string title, string name)
        {
            if(!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(name))
            {
                try
                {
                    await _priorityRepository.AddEntity(new TicketPriority { Name = name, Title = title });
                    await _priorityRepository.SaveEntity();
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }
               
            }
            return false;
        }

        public async Task<bool> DeletePriority(long priorityId)
        {
            var res = await _priorityRepository.RealDeleteEntity(priorityId);
            if(res)
                await _priorityRepository.SaveEntity();
            return res;
        }

        public async Task<List<TicketPriority>> GetAllPriority()
        {
            var priorities = await _priorityRepository.GetAllEntity().Where(p => p.IsRemove != false).ToListAsync();
            return priorities;
        }

        public Task<TicketPriority> GetPriority(long priorityId)
        {
            return _priorityRepository.GetEntity(priorityId);
        }

        public async Task<bool> UpdatePriority(long priorityId, string? title, string? name)
        {
            var priority =await _priorityRepository.GetEntity(priorityId);
            if(priority != null)
            {
                if (!string.IsNullOrWhiteSpace(title))
                    priority.Title = title;
                if (!string.IsNullOrWhiteSpace(name))
                    priority.Name = name;

                try
                {
                    _priorityRepository.UpDateEntity(priority);
                    await _priorityRepository.SaveEntity();
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }
            }
            return false;
        }
    }
}
