using OurSite.Core.DTOs.CheckBoxDtos;
using OurSite.Core.DTOs.TicketDtos;

namespace OurSite.Core.Services.Interfaces;

public interface ICheckBoxService
{
        Task<bool> CreateCheckBox(string title,string? IconName, string? Description,int SiteSection);
        Task<ResDeleteOpration> DeleteCheckBox(long priorityId);
        Task<CheckBoxDto> GetCheckBox(long priorityId);
        Task<bool> UpdateCheckBox(long priorityId, string? title, string? name);
        Task<List<CheckBoxDto>> GetAllCheckBox();
}
