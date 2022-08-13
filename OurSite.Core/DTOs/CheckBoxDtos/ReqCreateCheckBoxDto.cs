using OurSite.DataLayer.Entities.ConsultationRequest;

namespace OurSite.Core.DTOs.CheckBoxDtos;

public class ReqCreateCheckBoxDto
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public section SiteSectionId { get; set; } 
    public string? IconName { get; set; }
}
