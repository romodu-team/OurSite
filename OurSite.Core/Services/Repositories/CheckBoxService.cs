using System.Collections.Specialized;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using OurSite.Core.DTOs.CheckBoxDtos;
using OurSite.Core.DTOs.TicketDtos;
using OurSite.Core.Services.Interfaces;
using OurSite.DataLayer.Entities.ConsultationRequest;
using OurSite.DataLayer.Entities.Projects;
using OurSite.DataLayer.Interfaces;

namespace OurSite.Core.Services.Repositories;

public class CheckBoxService : ICheckBoxService
{
    #region Constructor
    private IGenericReopsitories<CheckBoxs> _CheckBoxRepository;
    private IGenericReopsitories<ItemSelected> _ConsultionItemSelectedRepository;
    private IGenericReopsitories<SelectedProjectPlan> _SelectedProjectPlanRepository;
    public CheckBoxService(IGenericReopsitories<ItemSelected> ConsultionItemSelectedRepository,IGenericReopsitories<SelectedProjectPlan> SelectedProjectPlanRepository,IGenericReopsitories<CheckBoxs> CheckBoxRepository)
    {
        _CheckBoxRepository=CheckBoxRepository;
        _ConsultionItemSelectedRepository=ConsultionItemSelectedRepository;
        _SelectedProjectPlanRepository=SelectedProjectPlanRepository;
    }
    #endregion
    public async Task<bool> CreateCheckBox(string title, string? IconName, string? Description, section SiteSection)
    {
        if(!string.IsNullOrEmpty(title))
            {
                try
                {
                    await _CheckBoxRepository.AddEntity(new CheckBoxs {CheckBoxName=title,Description=Description,IconName=IconName,sectionName=SiteSection });
                    await _CheckBoxRepository.SaveEntity();
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }
               
            }
            return false;
    }

    public async Task<ResDeleteOpration> DeleteCheckBox(List<long> CheckBoxId)
    {
        //check if checkbox used in project and consultion form 
        bool check=true;
        foreach (var checkbox in CheckBoxId)
        {
            var Resprojects = await _SelectedProjectPlanRepository.GetAllEntity().Where(t => t.CheckBoxId == checkbox && t.IsRemove == false).ToListAsync();
            var consultions = await _ConsultionItemSelectedRepository.GetAllEntity().Where(t => t.CheckBoxId == checkbox && t.IsRemove == false).ToListAsync();
            if ((Resprojects != null & Resprojects.Count > 0)||(consultions != null & consultions.Count > 0))
                return ResDeleteOpration.RefrenceError;
            else
            {
                check = await _CheckBoxRepository.RealDeleteEntity(checkbox);
            }
        }
       
        try
        {
            if (check)
            {
                await _CheckBoxRepository.SaveEntity();
                return ResDeleteOpration.Success;
            }
            return ResDeleteOpration.Failure;   
        }
        catch (Exception)
        {

            return ResDeleteOpration.Failure;
        }
                
    }

    public void Dispose()
    {
        _CheckBoxRepository.Dispose();
        _ConsultionItemSelectedRepository.Dispose();
        _SelectedProjectPlanRepository.Dispose();
    }

    public async Task<List<CheckBoxDto>> GetAllCheckBox()
    {
        var CheckBoxs = await _CheckBoxRepository.GetAllEntity().Where(p => p.IsRemove == false).Select(p=>new CheckBoxDto {Title=p.CheckBoxName,Description=p.Description,IconName=p.IconName,Id=p.Id,SiteSectionName=p.sectionName == section.ConsultationForm?"فرم مشاوره":"پروژه"}).ToListAsync();
            return CheckBoxs;
    }

    public async Task<CheckBoxDto> GetCheckBox(long CheckBoxId)
    {
         var CheckBox=await _CheckBoxRepository.GetEntity(CheckBoxId);
            if (CheckBox == null)
                return null;
            return new CheckBoxDto { Title=CheckBox.CheckBoxName,Description=CheckBox.Description,IconName=CheckBox.IconName,Id=CheckBox.Id,SiteSectionName=CheckBox.sectionName == section.ConsultationForm?"فرم مشاوره":"پروژه" };
    }

    public async Task<bool> UpdateCheckBox(long CheckBoxId, string? title, string? IconName, string? Description, int? SiteSection)
    {
        var CheckBox =await _CheckBoxRepository.GetEntity(CheckBoxId);
            if(CheckBox != null)
            {
                if (!string.IsNullOrWhiteSpace(title))
                    CheckBox.CheckBoxName = title;
                if (!string.IsNullOrWhiteSpace(IconName))
                    CheckBox.IconName = IconName;
                if (!string.IsNullOrWhiteSpace(Description))
                    CheckBox.Description = Description;
                if (SiteSection != null)
                    CheckBox.sectionName = (section)SiteSection;
                try
                {
                    _CheckBoxRepository.UpDateEntity(CheckBox);
                    await _CheckBoxRepository.SaveEntity();
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
