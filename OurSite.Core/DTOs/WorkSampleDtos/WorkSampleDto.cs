using System.Web.Mvc;

namespace OurSite.Core.DTOs.WorkSampleDtos;

public class WorkSampleDto
{
    public string ProjectName { get; set; }
    public string? EmployerName { get; set; }
    public string? CustomUrl { get; set; }
    [AllowHtml]
    public string? ShortDescription { get; set; }
    public string? LogoImagePath { get; set; }
    public string? HeaderImagePath{ get; set; }
    [AllowHtml]
    public string? Content { get; set; }

    public List<string>? ProjectFeatures { get; set; }
    public List<string>? DesignedPages { get; set; }
    public int Likes { get; set; }
    public List<string>? ImageGalleryPaths { get; set; }
    public List<long>? CategoriesId { get; set; }
}
