using OurSite.OurSite.Core.DTOs.WorkSampleDtos;

namespace OurSite.Core.Services.Interfaces;
public interface IWorkSampleCategoryService
{
    Task<bool> AddCategory(string Title,string Name);
    Task<bool> DeleteCategory(long categoryId);
    Task<bool> Editcategory(long categoryId,string? Title,string? Name);
    Task<List<GetWorkSampleCategoryDto>> GetAllCategories();
    Task<GetWorkSampleCategoryDto> GetCategory(long categoryId);
    Task<bool> AddCategoriesToWorkSample(long worksampleId,List<long> CategoriesId);
    Task<List<long>> GetWorkSamplesIdByCategory(List<long> categoriesId);
    Task<List<long>> GetCategoriesIdByWorkSampleId(long WorkSampleId);
    Task<bool> DeleteWorkSampleFromCategories(long WorkSampleId);

}