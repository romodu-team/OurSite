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
        private IGenericReopsitories<TicketModel> _ticketRepository;

        public TicketCategoryService(IGenericReopsitories<TicketCategory> ticketCategoryRepository, IGenericReopsitories<TicketModel> ticketRepository)
        {
            _TicketCategoryRepository = ticketCategoryRepository;
            _ticketRepository = ticketRepository;
        }

        public void Dispose()
        {
            _TicketCategoryRepository.Dispose();
            _ticketRepository.Dispose();
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

        public async Task<ResDeleteOpration> DeleteCategory(long CategoryId)
        {
            var tickets = await _ticketRepository.GetAllEntity().Where(t => t.TicketCategoryId == CategoryId && t.IsRemove == false).ToListAsync();
            if (tickets != null & tickets.Count > 0)
                return ResDeleteOpration.RefrenceError;
            else
            {
                try
                {
                    var res = await _TicketCategoryRepository.RealDeleteEntity(CategoryId);
                    if (res)
                    {
                        await _TicketCategoryRepository.SaveEntity();
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



        public async Task<List<TicketCategoryDto>> GetAllCategories()
        {
            var categories = await _TicketCategoryRepository.GetAllEntity().Where(p => p.IsRemove == false).Select(c=>new TicketCategoryDto { Id=c.Id,Title=c.Title,ParentId=c.ParentId,Name=c.Name}).ToListAsync();
            return categories;
        }

        public async Task<TicketCategoryDto> GetCategory(long CategoryId)
        {
            var category = await _TicketCategoryRepository.GetEntity(CategoryId);
            if (category == null)
                return null;
            return new TicketCategoryDto { Id=category.Id,Name=category.Name,ParentId=category.ParentId,Title=category.Title};
        }

        public async Task<bool> UpdateCategory(long CategoryId, string? title, string? name, long? parentId)
        {
            var Category = await _TicketCategoryRepository.GetEntity(CategoryId);
            if (Category != null)
            {
                if (!string.IsNullOrWhiteSpace(title))
                    Category.Title = title;
                if (!string.IsNullOrWhiteSpace(name))
                    Category.Name = name;
                if (parentId != null)
                {
                    if(await _TicketCategoryRepository.GetAllEntity().AnyAsync(c => c.Id == parentId))
                        Category.ParentId = parentId;
                    else
                        return false;
                }   
                    

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
