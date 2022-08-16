using Microsoft.EntityFrameworkCore;
using OurSite.Core.DTOs.TicketDtos;
using OurSite.Core.Services.Interfaces.TicketInterfaces;
using OurSite.DataLayer.Entities.Ticketing;
using OurSite.DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Services.Repositories.TicketServices
{
    public class TicketStatusService : ITicketStatusService
    {
        #region constructor
        private IGenericReopsitories<TicketStatus> _TicketStatusRepository;
        private IGenericReopsitories<TicketModel> _ticketRepository;

        public TicketStatusService(IGenericReopsitories<TicketStatus> ticketStatusRepository, IGenericReopsitories<TicketModel> ticketRepository)
        {
            _TicketStatusRepository = ticketStatusRepository;
            _ticketRepository = ticketRepository;
        }

        public void Dispose()
        {
            _TicketStatusRepository.Dispose();
            _ticketRepository.Dispose();
        }

        #endregion
        public async Task<bool> CreateStatus(string title, string name)
        {

            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(name))
            {
                try
                {
                    await _TicketStatusRepository.AddEntity(new TicketStatus { Name = name, Title = title });
                    await _TicketStatusRepository.SaveEntity();
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }

            }
            return false;
        }

        public async Task<ResDeleteOpration> DeleteStatus(long StatusId)
        {
            var tickets = await _ticketRepository.GetAllEntity().Where(t => t.TicketStatusId == StatusId && t.IsRemove == false).ToListAsync();
            if (tickets != null & tickets.Count > 0)
                return ResDeleteOpration.RefrenceError;
            else
            {
                try
                {
                    var res = await _TicketStatusRepository.RealDeleteEntity(StatusId);
                    if (res)
                    {
                        await _TicketStatusRepository.SaveEntity();
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



        public async Task<List<TicketStatusDto>> GetAllStatus()
        {
            var statusList = await _TicketStatusRepository.GetAllEntity().Where(p => p.IsRemove == false).Select(s=>new TicketStatusDto { StatusId = s.Id, Name = s.Name, Title = s.Title }).ToListAsync();
            return statusList;
        }

        public async Task<TicketStatusDto> GetStatus(long StatusId)
        {
            var status = await _TicketStatusRepository.GetEntity(StatusId);

            if (status == null)
                return null;
            return new TicketStatusDto { StatusId = status.Id, Name = status.Name, Title = status.Title };
        }

        public async Task<bool> UpdateStatus(long StatusId, string? title, string? name)
        {
            var Status = await _TicketStatusRepository.GetEntity(StatusId);
            if (Status != null)
            {
                if (!string.IsNullOrWhiteSpace(title))
                    Status.Title = title;
                if (!string.IsNullOrWhiteSpace(name))
                    Status.Name = name;

                try
                {
                    _TicketStatusRepository.UpDateEntity(Status);
                    await _TicketStatusRepository.SaveEntity();
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
