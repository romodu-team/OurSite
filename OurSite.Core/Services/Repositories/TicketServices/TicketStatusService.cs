using Microsoft.EntityFrameworkCore;
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
        public TicketStatusService(IGenericReopsitories<TicketStatus> ticketStatusRepository)
        {
            _TicketStatusRepository = ticketStatusRepository;
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

        public async Task<bool> DeleteStatus(long StatusId)
        {
            var res = await _TicketStatusRepository.RealDeleteEntity(StatusId);
            if (res)
                await _TicketStatusRepository.SaveEntity();
            return res;
        }

        public async Task<List<TicketStatus>> GetAllStatus()
        {
            var statusList = await _TicketStatusRepository.GetAllEntity().Where(p => p.IsRemove != false).ToListAsync();
            return statusList;
        }

        public async Task<TicketStatus> GetStatus(long StatusId)
        {
            var status = await _TicketStatusRepository.GetEntity(StatusId);
            return status;
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
