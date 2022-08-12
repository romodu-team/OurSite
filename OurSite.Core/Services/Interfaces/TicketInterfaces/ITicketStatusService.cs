using OurSite.Core.DTOs.TicketDtos;


namespace OurSite.Core.Services.Interfaces.TicketInterfaces
{
    public interface ITicketStatusService
    {
        Task<bool> CreateStatus(string title, string name);
        Task<ResDeleteOpration> DeleteStatus(long StatusId);
        Task<TicketStatusDto> GetStatus(long StatusId);
        Task<bool> UpdateStatus(long StatusId, string? title, string? name);
        Task<List<TicketStatusDto>> GetAllStatus();
    }
}
