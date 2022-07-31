namespace OurSite.Core.DTOs.WorkSampleDtos;

public class GetAllWorkSampleDto
{
    public string ProjectName { get; set; }
    public string? LogoPath { get; set; }
    public int Like { get; set; }
    public string? CoustomUrl { get; set; }
    public List<string>? FeaturesList { get; set; }
}