using OurSite.DataLayer.Entities.Ticketing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Services.Interfaces.TicketInterfaces
{
    public interface ITicketCategoryService
    {
        Task<bool> CreateCategory(string title, string name);
        Task<bool> DeleteCategory(long CategoryId);
        Task<TicketCategory> GetCategory(long CategoryId);
        Task<bool> UpdateCategory(long CategoryId, string? title, string? name);
        Task<List<TicketCategory>> GetAllCategories();
    }
}
