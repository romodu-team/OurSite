using System.Web.Mvc;
using Microsoft.AspNetCore.Http;

namespace OurSite.Core.DTOs.WorkSampleDtos;

public class CreateWorkSampleDto
{
     public string ProjectName { get; set; }
    public string? EmployerName { get; set; }
    public string? CustomUrl { get; set; }
    [AllowHtml]
    public string? ShortDescription { get; set; }
    public IFormFile? LogoImageName { get; set; }
    public IFormFile? HeaderImageName { get; set; }
    [AllowHtml]
    public string? Content { get; set; }
    public List<string>? ProjectFeatures { get; set; }
    public List<string>? DesignedPages { get; set; }

}
public enum ResWorkSample{
    Success,
    Faild
}
