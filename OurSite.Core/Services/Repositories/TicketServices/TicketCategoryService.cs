using OurSite.Core.DTOs.TicketDtos;
using OurSite.Core.Services.Interfaces.TicketInterfaces;
using OurSite.DataLayer.Entities.Ticketing;
using OurSite.DataLayer.Interfaces;
using OurSite.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Services.Repositories.TicketServices
{
    public class TicketCategoryService: ITicketCategoryService
    {
        #region constructor
        private IGenericReopsitories<TicketCategory> _TicketCategoryRepository;
        public TicketCategoryService(IGenericReopsitories<TicketCategory> ticketCategoryRepository)
        {
            _TicketCategoryRepository = ticketCategoryRepository;
        }

        #endregion
        public async Task<ResGetCategoryDto> GetCategory(long CategoryId)
        {
            var category = await _TicketCategoryRepository.GetEntity(CategoryId);
            if (category is not null)
                return new ResGetCategoryDto { CreateDate = category.CreateDate.ToShortDateString(), Id = category.Id, LastUpdateDate = category.LastUpdate.ToShortDateString(), Name = category.Name, Title = category.Title };
            return null;
        }
    }
}
