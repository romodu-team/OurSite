using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> CreateCategory(string title, string name)
        {
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(name))
            {
                try
                {
                    await _TicketCategoryRepository.AddEntity(new TicketCategory { Name = name, Title = title });
                    await _TicketCategoryRepository.SaveEntity();
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }

            }
            return false;
        }

        public async Task<bool> DeleteCategory(long CategoryId)
        {
            var res = await _TicketCategoryRepository.RealDeleteEntity(CategoryId);
            if (res)
                await _TicketCategoryRepository.SaveEntity();
            return res;
        }

        public async Task<List<TicketCategory>> GetAllCategories()
        {
            var priorities = await _TicketCategoryRepository.GetAllEntity().Where(p => p.IsRemove != false).ToListAsync();
            return priorities;
        }

        public async Task<TicketCategory> GetCategory(long CategoryId)
        {
            var category = await _TicketCategoryRepository.GetEntity(CategoryId);
            return category;
        }

        public async Task<bool> UpdateCategory(long CategoryId, string? title, string? name)
        {
            var Category = await _TicketCategoryRepository.GetEntity(CategoryId);
            if (Category != null)
            {
                if (!string.IsNullOrWhiteSpace(title))
                    Category.Title = title;
                if (!string.IsNullOrWhiteSpace(name))
                    Category.Name = name;

                try
                {
                    _TicketCategoryRepository.UpDateEntity(Category);
                    await _TicketCategoryRepository.SaveEntity();
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
