using OurSite.Core.DTOs.TicketDtos;

namespace OurSite.Core.Services.Interfaces.TicketInterfaces
{
    public interface ITicketCategoryService
    {
        Task<bool> CreateCategory(string title, string name);
        Task<ResDeleteOpration> DeleteCategory(long CategoryId);
        Task<TicketCategoryDto> GetCategory(long CategoryId);
        Task<bool> UpdateCategory(long CategoryId, string? title, string? name,long? parentId);
        Task<List<TicketCategoryDto>> GetAllCategories();
    }
}
