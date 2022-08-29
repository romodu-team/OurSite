namespace OurSite.Core.DTOs.CheckBoxDtos;

public class ReqUpdateCheckBox
{
    public long CheckBoxId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? SiteSectionId { get; set; } 
    public string? IconName { get; set; }
}
