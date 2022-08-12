using OurSite.Core.Services.Interfaces.TicketInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OurSite.DataLayer.Interfaces;
using OurSite.DataLayer.Entities.Ticketing;
using Microsoft.EntityFrameworkCore;
using OurSite.Core.DTOs.TicketDtos;

namespace OurSite.Core.Services.Repositories.TicketServices
{
    public class TicketPriorityService : ITicketPriorityService
    {
        private IGenericReopsitories<TicketPriority> _priorityRepository;
        private IGenericReopsitories<TicketModel> _ticketRepository;
        public TicketPriorityService(IGenericReopsitories<TicketModel> ticketRepository,IGenericReopsitories<TicketPriority> PriorityRepository)
        {
            _priorityRepository = PriorityRepository;
            _ticketRepository = ticketRepository;
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

        public async Task<ResDeleteOpration> DeletePriority(long priorityId)
        {
            var tickets = await _ticketRepository.GetAllEntity().Where(t => t.TicketPriorityId == priorityId && t.IsRemove == false).ToListAsync();
            if (tickets != null & tickets.Count > 0)
                return ResDeleteOpration.RefrenceError;
            else
            {
                try
                {
                    var res = await _priorityRepository.RealDeleteEntity(priorityId);
                    if (res)
                    {
                        await _priorityRepository.SaveEntity();
                        return ResDeleteOpration.Success;
                    }
                    return ResDeleteOpration.Failure;   
                }
                catch (Exception)
                {

                    return ResDeleteOpration.Failure;
                }
                
            }
            
        }

        public async Task<List<TicketPriorityDto>> GetAllPriority()
        {
            var priorities = await _priorityRepository.GetAllEntity().Where(p => p.IsRemove == false).Select(p=>new TicketPriorityDto { Title=p.Title,PriorityId=p.Id,Name=p.Name}).ToListAsync();
            return priorities;
        }

        public async Task<TicketPriorityDto> GetPriority(long priorityId)
        {
            var priority=await _priorityRepository.GetEntity(priorityId);
            if (priority == null)
                return null;
            return new TicketPriorityDto { Name = priority.Name, PriorityId = priority.Id, Title = priority.Title };
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
