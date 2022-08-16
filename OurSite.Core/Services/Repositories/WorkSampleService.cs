using Microsoft.EntityFrameworkCore;
using OurSite.Core.DTOs.Paging;
using OurSite.Core.DTOs.WorkSampleDtos;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Utilities;
using OurSite.Core.Utilities.Extentions.Paging;
using OurSite.DataLayer.Entities.RatingModel;
using OurSite.DataLayer.Entities.WorkSamples;
using OurSite.DataLayer.Interfaces;

namespace OurSite.Core.Services.Repositories;

public class WorkSampleService : IWorkSampleService
{
    private IGenericRepository<WorkSample> _WorkSampleRepository;
    private IGenericRepository<ProjectFeatures> _ProjectFeatureRepository;
    private IGenericRepository<Like> _LikeRepository;
    private IimageGalleryService _ImageGalleryService;
    private IWorkSampleCategoryService _IworkSampleCategoryService;
    public WorkSampleService(IGenericRepository<WorkSample> WorkSampleRepository, IGenericRepository<ProjectFeatures> ProjectFeatureRepository, IGenericRepository<Like> LikeRepository, IimageGalleryService ImageGalleryService, IWorkSampleCategoryService IworkSampleCategoryService)
    {
        _IworkSampleCategoryService = IworkSampleCategoryService;
        _ProjectFeatureRepository = ProjectFeatureRepository;
        _WorkSampleRepository = WorkSampleRepository;
        _LikeRepository = LikeRepository;
        _ImageGalleryService = ImageGalleryService;
    }

    public void Dispose()
    {
        _WorkSampleRepository.Dispose();
        _ProjectFeatureRepository.Dispose();
        _LikeRepository.Dispose();
        _ImageGalleryService.Dispose();
        _IworkSampleCategoryService.Dispose();
    }

    public async Task<ResCreateWorkSampleDto> CreateWorkSample(CreateWorkSampleDto request)
    {
        WorkSample worksample = new WorkSample()
        {
            Content = request.Content,
            CustomUrl = request.CustomUrl,
            EmployerName = request.EmployerName,
            ProjectName = request.ProjectName,
            ShortDescription = request.ShortDescription

        };
        try
        {
            if (request.HeaderImage is not null)
            {
                var res = await FileUploader.UploadFile(PathTools.WorkSampleImages, request.HeaderImage, 10);
                if (res.Status != resFileUploader.Success)
                    return new ResCreateWorkSampleDto { WorkSampleID = -1, resCreateWorkSample = res.Status };
                worksample.HeaderImageName = res.FileName;
            }
            if (request.LogoImage is not null)
            {
                var res = await FileUploader.UploadFile(PathTools.WorkSampleImages, request.LogoImage, 10);
                if (res.Status != resFileUploader.Success)
                    return new ResCreateWorkSampleDto { WorkSampleID = -1, resCreateWorkSample = res.Status };
                worksample.LogoImageName = res.FileName;
            }

            await _WorkSampleRepository.AddEntity(worksample);
            await _WorkSampleRepository.SaveEntity();

            if (request.ProjectFeatures is not null)
            {
                var list = request.ProjectFeatures[0].Split(",");
                foreach (var item in list)
                {
                    ProjectFeatures features = new ProjectFeatures()
                    {
                        FeatureType = WorkSampleFeatureType.ProjectFeatured,
                        Title = item,
                        WorkSampleId = worksample.Id
                    };
                    await _ProjectFeatureRepository.AddEntity(features);
                }
                await _ProjectFeatureRepository.SaveEntity();
            }
            if (request.DesignedPages is not null)
            {
                var list = request.DesignedPages[0].Split(",");
                foreach (var item in list)
                {
                    ProjectFeatures features = new ProjectFeatures()
                    {
                        FeatureType = WorkSampleFeatureType.DesignedPages,
                        Title = item,
                        WorkSampleId = worksample.Id
                    };
                    await _ProjectFeatureRepository.AddEntity(features);
                }
                await _ProjectFeatureRepository.SaveEntity();
            }
            if (request.CategoriesId is not null)
            {
                var list = request.CategoriesId[0].Split(",");
                var categoriesID = new List<long>();
                foreach (var item in list)
                {
                    categoriesID.Add(Convert.ToInt64(item));
                }
                await _IworkSampleCategoryService.AddCategoriesToWorkSample(worksample.Id, categoriesID);

            }


            return new ResCreateWorkSampleDto { WorkSampleID = worksample.Id, resCreateWorkSample = resFileUploader.Success };
        }
        catch (System.Exception)
        {

            return new ResCreateWorkSampleDto { WorkSampleID = -1, resCreateWorkSample = resFileUploader.Failure };
        }
    }

    public async Task<ResWorkSample> DeleteWorkSample(long WorkSampleId)
    {

        try
        {
            //delete worksample
            var resDelWorkSample = await _WorkSampleRepository.DeleteEntity(WorkSampleId);
            if (!resDelWorkSample)
                return ResWorkSample.Faild;
            //delete worksample image gallery
            var ImageGalleries = await _ImageGalleryService.GetActiveGalleryByWorkSampleId(WorkSampleId);
            if (ImageGalleries.Any())
            {
                foreach (var Image in ImageGalleries)
                {
                    await _ImageGalleryService.DeleteImageFromGallery(Image.Id);
                }
            }
            //delete worksample like
            var likes = await _LikeRepository.GetAllEntity().Where(l => l.WorkSampleId == WorkSampleId).ToListAsync();
            if (likes.Any())
            {
                foreach (var like in likes)
                {
                    await _LikeRepository.DeleteEntity(like.Id);
                }
            }
            //delete worksample in category
            var res = await _IworkSampleCategoryService.DeleteWorkSampleFromCategories(WorkSampleId);
            if (!res)
                return ResWorkSample.Faild;
            //delete project features
            var ProjectFeatures = await _ProjectFeatureRepository.GetAllEntity().Where(p => p.WorkSampleId == WorkSampleId).ToListAsync();
            if (ProjectFeatures.Any())
            {
                foreach (var feature in ProjectFeatures)
                {
                    await _ProjectFeatureRepository.DeleteEntity(feature.Id);
                }
            }

            await _LikeRepository.SaveEntity();
            await _ProjectFeatureRepository.SaveEntity();
            await _WorkSampleRepository.SaveEntity();
            return ResWorkSample.Success;
        }
        catch (System.Exception)
        {

            return ResWorkSample.Faild;
        }



    }


    public async Task<ResWorkSample> EditWorkSample(long worksampleId, EditWorkSampleDto request)
    {
        //find work sample 
        var worksample = await _WorkSampleRepository.GetEntity(worksampleId);
        if (worksample is not null)
        {

            try
            {
                if (request.HeaderImage is not null)
                {
                    var res = await FileUploader.UploadFile(PathTools.WorkSampleImages, request.HeaderImage, 10);
                    if (res.Status != resFileUploader.Success)
                        return ResWorkSample.Faild;
                    worksample.HeaderImageName = res.FileName;
                }
                if (request.LogoImage is not null)
                {
                    var res = await FileUploader.UploadFile(PathTools.WorkSampleImages, request.LogoImage, 10);
                    if (res.Status != resFileUploader.Success)
                        return ResWorkSample.Faild;
                    worksample.LogoImageName = res.FileName;
                }

                if (!string.IsNullOrWhiteSpace(request.Content))
                    worksample.Content = request.Content;
                if (!string.IsNullOrWhiteSpace(request.CustomUrl))
                    worksample.CustomUrl = request.CustomUrl;
                if (!string.IsNullOrWhiteSpace(request.EmployerName))
                    worksample.EmployerName = request.EmployerName;
                if (!string.IsNullOrWhiteSpace(request.ProjectName))
                    worksample.ProjectName = request.ProjectName;
                if (!string.IsNullOrWhiteSpace(request.ShortDescription))
                    worksample.ShortDescription = request.ShortDescription;

                _WorkSampleRepository.UpDateEntity(worksample);
                await _WorkSampleRepository.SaveEntity();

                if (request.ProjectFeatures is not null)
                {
                    //delete all exist project features
                    var ExitedProjectFeatures = await _ProjectFeatureRepository.GetAllEntity().Where(p => p.WorkSampleId == worksample.Id && p.FeatureType == WorkSampleFeatureType.ProjectFeatured).Select(p => p.Id).ToListAsync();
                    foreach (var featureId in ExitedProjectFeatures)
                    {
                        await _ProjectFeatureRepository.RealDeleteEntity(featureId);
                    }
                    //add new project features
                    var list = request.ProjectFeatures[0].Split(",");

                    foreach (var item in list)
                    {
                        ProjectFeatures features = new ProjectFeatures()
                        {
                            FeatureType = WorkSampleFeatureType.ProjectFeatured,
                            Title = item,
                            WorkSampleId = worksample.Id
                        };
                        await _ProjectFeatureRepository.AddEntity(features);
                    }
                    await _ProjectFeatureRepository.SaveEntity();
                }
                if (request.DesignedPages is not null)
                {
                    //delete all exist Designed Pages
                    var ExitedDesignedPages = await _ProjectFeatureRepository.GetAllEntity().Where(p => p.WorkSampleId == worksample.Id && p.FeatureType == WorkSampleFeatureType.DesignedPages).Select(p => p.Id).ToListAsync();
                    foreach (var DesignedPageId in ExitedDesignedPages)
                    {
                        await _ProjectFeatureRepository.RealDeleteEntity(DesignedPageId);
                    }
                    //add new Designed Pages
                    var list = request.DesignedPages[0].Split(",");
                    foreach (var item in list)
                    {
                        ProjectFeatures features = new ProjectFeatures()
                        {
                            FeatureType = WorkSampleFeatureType.DesignedPages,
                            Title = item,
                            WorkSampleId = worksample.Id
                        };
                        await _ProjectFeatureRepository.AddEntity(features);
                    }
                    await _ProjectFeatureRepository.SaveEntity();

                }

                if (request.CategoriesId is not null)
                {
                    //add new 
                    var list = request.CategoriesId[0].Split(",");
                    var CategoriesId = new List<long>();
                    foreach (var item in list)
                    {
                        CategoriesId.Add(Convert.ToInt64(item));
                    }
                    await _IworkSampleCategoryService.AddCategoriesToWorkSample(worksampleId, CategoriesId);

                }

                return ResWorkSample.Success;
            }
            catch (System.Exception)
            {

                return ResWorkSample.Faild;
            }
        }
        return ResWorkSample.NotFound;

    }

    public async Task<ResFilterWorkSampleDto> GetAllWorkSamples(ReqFilterWorkSampleDto request)
    {
        var WorkSampleQuery = _WorkSampleRepository.GetAllEntity().Where(w => w.IsRemove == false).AsQueryable();

        //orderby
        if (request.OrderBy is not null)
        {
            switch (request.OrderBy)
            {
                case WorkSampleOrderBy.DateAsc:
                    WorkSampleQuery = WorkSampleQuery.OrderBy(w => w.CreateDate);
                    break;
                case WorkSampleOrderBy.DateDec:
                    WorkSampleQuery = WorkSampleQuery.OrderByDescending(w => w.CreateDate);
                    break;
                case WorkSampleOrderBy.LikeAsc://check if likes is null for errors
                    WorkSampleQuery = WorkSampleQuery.Include(w => w.Likes).OrderBy(w => w.Likes.Count());
                    break;
                case WorkSampleOrderBy.LikeDec:
                    WorkSampleQuery = WorkSampleQuery.Include(w => w.Likes).OrderByDescending(w => w.Likes.Count());
                    break;
                default:
                    break;
            }
        }
        //category

        if (request.CategoriesId is not null)
        {
            //بدست اوردن ای دی نمونه کار هایی که تو اون دسته بندی ها هستن
            var ListOfWorkSamplesId = await _IworkSampleCategoryService.GetWorkSamplesIdByCategory(request.CategoriesId);
            //گرفتن نمونه کار هایی که ای دی شون تو لیست بالاییه
            WorkSampleQuery = WorkSampleQuery.Where(w => ListOfWorkSamplesId.Contains(w.Id));
        }

        //pagination

        var count = (int)Math.Ceiling(WorkSampleQuery.Count() / (double)request.TakeEntity);
        var pager = Pager.Build(count, request.PageId, request.TakeEntity);

        var list = await WorkSampleQuery.Include(u => u.ProjectFeatures.Where
        (p => p.FeatureType == WorkSampleFeatureType.ProjectFeatured))
        .Paging(pager).Select(u => new GetAllWorkSampleDto { Id = u.Id, CoustomUrl = u.CustomUrl, LogoPath = PathTools.GetWorkSampleImages + u.LogoImageName, ProjectName = u.ProjectName, Like = u.Likes.Count(), FeaturesList = u.ProjectFeatures.Select(o => o.Title).Take(4).ToList() }).ToListAsync();

        var result = new ResFilterWorkSampleDto();
        result.SetPaging(pager);
        return result.SetWorkSamples(list);
    }

    public async Task<WorkSampleDto> GetWorkSample(long WorkSampleId)
    {
        //get worksample
        var WorkSample = await _WorkSampleRepository.GetEntity(WorkSampleId);
        if (WorkSample is not null)
        {
            var worksampleDto = new WorkSampleDto()
            {
                Content = WorkSample.Content,
                CustomUrl = WorkSample.CustomUrl,
                EmployerName = WorkSample.EmployerName,
                HeaderImagePath = WorkSample.HeaderImageName is not null ? PathTools.GetWorkSampleImages + WorkSample.HeaderImageName : null,
                LogoImagePath = WorkSample.LogoImageName is not null ? PathTools.GetWorkSampleImages + WorkSample.LogoImageName : null,
                ProjectName = WorkSample.ProjectName,
                ShortDescription = WorkSample.ShortDescription,
                ProjectFeatures = new List<string>(),
                DesignedPages = new List<string>(),
                ImageGalleryPaths = new List<string>()
            };
            //get imageGallery
            var ImageGallary = await _ImageGalleryService.GetActiveGalleryByWorkSampleId(WorkSampleId);
            if (ImageGallary.Any())
            {
                foreach (var Image in ImageGallary)
                {
                    worksampleDto.ImageGalleryPaths.Add(Image.ImagePath);
                }
            }
            //get likes
            int Like = _LikeRepository.GetAllEntity().Count(l => l.WorkSampleId == WorkSampleId && l.IsRemove == false);
            worksampleDto.Likes = Like > 0 ? Like : 0;
            //get project features
            var ProjectFeatures = await _ProjectFeatureRepository.GetAllEntity().Where(p => p.FeatureType == WorkSampleFeatureType.ProjectFeatured && p.WorkSampleId == WorkSampleId && p.IsRemove == false).ToListAsync();

            if (ProjectFeatures.Any())
            {
                foreach (var Feature in ProjectFeatures)
                {
                    worksampleDto.ProjectFeatures.Add(Feature.Title);
                }
            }
            //get Designed pages
            var DesignedPages = await _ProjectFeatureRepository.GetAllEntity().Where(p => p.FeatureType == WorkSampleFeatureType.DesignedPages && p.WorkSampleId == WorkSampleId && p.IsRemove == false).ToListAsync();

            if (DesignedPages.Any())
            {
                foreach (var Pages in DesignedPages)
                {
                    worksampleDto.DesignedPages.Add(Pages.Title);
                }
            }
            //get categories
            var Categories = await _IworkSampleCategoryService.GetCategoriesIdByWorkSampleId(WorkSampleId);
            worksampleDto.CategoriesId = Categories;
            return worksampleDto;
        }
        return null;

    }
}
