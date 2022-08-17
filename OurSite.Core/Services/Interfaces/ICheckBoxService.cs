using System.Collections.Specialized;
using System;
using OurSite.Core.DTOs.CheckBoxDtos;
using OurSite.Core.DTOs.TicketDtos;
using OurSite.DataLayer.Entities.ConsultationRequest;

namespace OurSite.Core.Services.Interfaces;

public interface ICheckBoxService:IDisposable
{
        Task<bool> CreateCheckBox(string title,string? IconName, string? Description,section SiteSection);
        Task<ResDeleteOpration> DeleteCheckBox(List<long> CheckBoxId);
        Task<CheckBoxDto> GetCheckBox(long CheckBoxId);
        Task<bool> UpdateCheckBox(long CheckBoxId, string? title,string? IconName, string? Description,int? SiteSection);
        Task<List<CheckBoxDto>> GetAllCheckBox(string sectionId);
}
