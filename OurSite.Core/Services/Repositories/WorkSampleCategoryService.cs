using Microsoft.EntityFrameworkCore;
using OurSite.Core.Services.Interfaces;
using OurSite.DataLayer.Entities.WorkSamples;
using OurSite.DataLayer.Interfaces;
using OurSite.OurSite.Core.DTOs.WorkSampleDtos;
using OurSite.OurSite.DataLayer.Entities.WorkSamples;

namespace OurSite.OurSite.Core.Services.Repositories;
public class WorkSampleCategoryService : IWorkSampleCategoryService
{
    private IGenericRepository<WorkSampleInCategory> _WorkSampleInCategoryReopsitory;
    private IGenericRepository<WorkSampleCategory> _WorkSampleCategoryRepository;
    public WorkSampleCategoryService(IGenericRepository<WorkSampleInCategory> WorkSampleInCategoryReopsitory,IGenericRepository<WorkSampleCategory> WorkSampleCategoryRepository)
    {
        _WorkSampleCategoryRepository=WorkSampleCategoryRepository;
        _WorkSampleInCategoryReopsitory=WorkSampleInCategoryReopsitory;
    }

    public void Dispose()
    {
        _WorkSampleInCategoryReopsitory.Dispose();
        _WorkSampleCategoryRepository.Dispose();
    }


    public async Task<bool> AddCategoriesToWorkSample(long worksampleId, List<long> CategoriesId)
    {
        //delete all existed worksampleincategories
        var alreadyWorkSampleInCategoryList= await _WorkSampleInCategoryReopsitory.GetAllEntity().Where(w=>w.WorkSampleId==worksampleId).ToListAsync();
        foreach (var item in alreadyWorkSampleInCategoryList)
        {
            await _WorkSampleInCategoryReopsitory.RealDeleteEntity(item.Id);
        }
        await _WorkSampleInCategoryReopsitory.SaveEntity();

        //add new categories to worksample
        try
        {
            foreach (var categoryId in CategoriesId)
            {
                if(await _WorkSampleCategoryRepository.GetAllEntity().AnyAsync(c => c.Id == categoryId))
                {
                    var workSampleIncategory = new WorkSampleInCategory()
                    {
                        WorkSampleCategoryId = categoryId,
                        WorkSampleId = worksampleId
                    };
                    await _WorkSampleInCategoryReopsitory.AddEntity(workSampleIncategory);
                }
                
            }
            await _WorkSampleInCategoryReopsitory.SaveEntity();
            return true;
        }
        catch (System.Exception)
        {
            return false;
        }
        
    }

    public async Task<bool> AddCategory(string Title, string Name)
    {
        if(string.IsNullOrWhiteSpace(Title))
            return false;
        if(string.IsNullOrWhiteSpace(Name))
            return false;
        var category = new WorkSampleCategory(){
            Name=Name,
            Title=Title
        };
        try
        {
            await _WorkSampleCategoryRepository.AddEntity(category);
            await _WorkSampleCategoryRepository.SaveEntity();
            return true;
        }
        catch (System.Exception)
        {
            return false;
            
        }
    }

    public async Task<bool> DeleteCategory(long categoryId)
    {
       
        try
        {
            //get list of workasmple in category
            var list = await _WorkSampleInCategoryReopsitory.GetAllEntity().Where(c => c.WorkSampleCategoryId == categoryId).ToListAsync();
            //delete each element of list
            foreach (var item in list)
            {
                await _WorkSampleInCategoryReopsitory.RealDeleteEntity(item.Id);
            }
            await _WorkSampleInCategoryReopsitory.SaveEntity();
            //delete category
            await _WorkSampleCategoryRepository.RealDeleteEntity(categoryId);
            await _WorkSampleCategoryRepository.SaveEntity();
            return true;

        }
        catch (System.Exception)
        {
            
            return false;
        }
    }

    public async Task<bool> DeleteWorkSampleFromCategories(long WorkSampleId)
    {
        var list =await _WorkSampleInCategoryReopsitory.GetAllEntity().Where(w=>w.WorkSampleId==WorkSampleId).ToListAsync();

        foreach (var item in list)
        {
            await _WorkSampleInCategoryReopsitory.RealDeleteEntity(item.Id);
        }
        try
        {
            await _WorkSampleInCategoryReopsitory.SaveEntity();
            return true;
        }
        catch (System.Exception)
        {
            
            return false;
        }
        
    }



    public async Task<bool> Editcategory(long categoryId, string? Title, string? Name)
    {
        var category = await _WorkSampleCategoryRepository.GetEntity(categoryId);
        if(category is not null)
        {
            if(!string.IsNullOrWhiteSpace(Title))
                category.Title=Title;
            if(!string.IsNullOrWhiteSpace(Name))
                category.Name=Name;
            try
            {
                _WorkSampleCategoryRepository.UpDateEntity(category);
                await _WorkSampleCategoryRepository.SaveEntity();
                return true;
            }
            catch (System.Exception)
            {
                
                return false;
            }
        }
        return false;
    }

    public async Task<List<GetWorkSampleCategoryDto>> GetAllCategories()
    {
        var categories = await _WorkSampleCategoryRepository.GetAllEntity().Select(c=>new GetWorkSampleCategoryDto(){CategoryId=c.Id,Name=c.Name,Title=c.Title}).ToListAsync();

        return categories;
    }

    public async Task<List<long>> GetCategoriesIdByWorkSampleId(long WorkSampleId)
    {
        var categoriesId = await _WorkSampleInCategoryReopsitory.GetAllEntity().Where(c => c.WorkSampleId == WorkSampleId).Select(c => c.WorkSampleCategoryId).ToListAsync();
        return categoriesId;
    }

    public async Task<GetWorkSampleCategoryDto> GetCategory(long categoryId)
    {
        var category = await _WorkSampleCategoryRepository.GetEntity(categoryId);
        if(category is not null)
        {
            return new GetWorkSampleCategoryDto(){CategoryId=category.Id,Name=category.Name,Title=category.Title};
        }
        return null;
    }

    public async Task<List<long>> GetWorkSamplesIdByCategory(List<long> categoriesId)
    {
        var WorkSamplesIdList = await _WorkSampleInCategoryReopsitory.GetAllEntity().Where(c=>categoriesId.Contains(c.WorkSampleCategoryId)).Select(c=>c.WorkSampleId).ToListAsync();
        return WorkSamplesIdList;
    }
}