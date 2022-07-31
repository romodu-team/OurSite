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
    private IGenericReopsitories<WorkSample> _WorkSampleRepository;
    private IGenericReopsitories<ProjectFeatures> _ProjectFeatureRepository;
    private IGenericReopsitories<Like> _LikeRepository;
    private IimageGalleryService _ImageGalleryService;

    public WorkSampleService(IGenericReopsitories<WorkSample> WorkSampleRepository,IGenericReopsitories<ProjectFeatures> ProjectFeatureRepository,IGenericReopsitories<Like> LikeRepository,IimageGalleryService ImageGalleryService)
    {
        _ProjectFeatureRepository=ProjectFeatureRepository;
        _WorkSampleRepository=WorkSampleRepository;
        _LikeRepository=LikeRepository;
        _ImageGalleryService=ImageGalleryService;
    }
    
    public async Task<ResCreateWorkSampleDto> CreateWorkSample(CreateWorkSampleDto request)
    {
        WorkSample worksample = new WorkSample()
        {
            Content=request.Content,
            CustomUrl=request.CustomUrl,
            EmployerName=request.EmployerName,
            ProjectName=request.ProjectName,
            ShortDescription=request.ShortDescription

        };
        try
        {    if(request.HeaderImage is not null){
               var res= await FileUploader.UploadFile(PathTools.WorkSampleImages,request.HeaderImage,10);
               if(res.Status!=resFileUploader.Success)
                    return new ResCreateWorkSampleDto{WorkSampleID=-1,resCreateWorkSample=res.Status};
                worksample.HeaderImageName=res.FileName;
            }
             if(request.LogoImage is not null){
               var res= await FileUploader.UploadFile(PathTools.WorkSampleImages,request.LogoImage,10);
               if(res.Status!=resFileUploader.Success)
                    return new ResCreateWorkSampleDto{WorkSampleID=-1,resCreateWorkSample=res.Status};
               worksample.LogoImageName=res.FileName;
            }
            
            await _WorkSampleRepository.AddEntity(worksample);
            await _WorkSampleRepository.SaveEntity();

            if(request.ProjectFeatures is not null)
            {
                foreach (var item in request.ProjectFeatures)
                {
                    ProjectFeatures features = new ProjectFeatures(){
                        FeatureType=WorkSampleFeatureType.ProjectFeatured,
                        Title=item,
                        WorkSampleId=worksample.Id
                    };
                  await  _ProjectFeatureRepository.AddEntity(features);
                }
                await _ProjectFeatureRepository.SaveEntity();
            }
            if(request.DesignedPages is not null)
            {
                foreach (var item in request.DesignedPages)
                {
                    ProjectFeatures features = new ProjectFeatures(){
                        FeatureType=WorkSampleFeatureType.DesignedPages,
                        Title=item,
                        WorkSampleId=worksample.Id
                    };
                  await  _ProjectFeatureRepository.AddEntity(features);
                }
                await _ProjectFeatureRepository.SaveEntity();
            }

           
        
            return new ResCreateWorkSampleDto{WorkSampleID=worksample.Id,resCreateWorkSample=resFileUploader.Success};
        }
        catch (System.Exception)
        {
            
            return new ResCreateWorkSampleDto{WorkSampleID=-1,resCreateWorkSample=resFileUploader.Failure};
        }
    }   

    public Task<ResWorkSample> DeleteWorkSample(DeleteWorkSampleDto request)
    {
        throw new NotImplementedException();
    }

    public Task<ResWorkSample> EditWorkSample(EditWorkSampleDto request)
    {
        throw new NotImplementedException();
    }

    public async Task<ResFilterWorkSampleDto> GetAllWorkSamples(ReqFilterWorkSampleDto request)
    {
        var WorkSampleQuery= _WorkSampleRepository.GetAllEntity().Where(w=>w.IsRemove==false).AsQueryable();

        //orderby
        if(request.OrderBy is not null){
            switch (request.OrderBy)
            {
                case WorkSampleOrderBy.DateAsc:
                    WorkSampleQuery=WorkSampleQuery.OrderBy(w=>w.CreateDate);
                    break;
                case WorkSampleOrderBy.DateDec:
                    WorkSampleQuery=WorkSampleQuery.OrderByDescending(w=>w.CreateDate);
                    break;
                case WorkSampleOrderBy.LikeAsc://check if likes is null for errors
                    WorkSampleQuery=WorkSampleQuery.Include(w=>w.Likes).OrderBy(w=>w.Likes.Count());
                    break;
                case WorkSampleOrderBy.LikeDec:
                    WorkSampleQuery=WorkSampleQuery.Include(w=>w.Likes).OrderByDescending(w=>w.Likes.Count());
                    break;
                default:
                    break;
            }
        }
        //category

            //بدست اوردن ای دی نمونه کار هایی که تو اون دسته بندی ها هستن
            //گرفتن نمونه کار هایی که ای دی شون تو لیست بالاییه
            if(request.CategoriesId is not null){
                WorkSampleQuery= WorkSampleQuery.Include(w=>w.workSampleInCategories).SelectMany(x=>x.workSampleInCategories.Where(c=>request.CategoriesId.Contains(c.WorkSampleCategoryId)));
            }
           
        //pagination

        var count = (int)Math.Ceiling(WorkSampleQuery.Count() / (double)request.TakeEntity);
            var pager = Pager.Build(count, request.PageId, request.TakeEntity);

            var list =await WorkSampleQuery.Include(u=>u.ProjectFeatures.Where
            (p=>p.FeatureType==WorkSampleFeatureType.ProjectFeatured))
            .Paging(pager).Select(u=> new GetAllWorkSampleDto {CoustomUrl=u.CustomUrl,LogoPath=PathTools.GetWorkSampleImages+u.LogoImageName,ProjectName=u.ProjectName,Like=u.Likes.Count(),FeaturesList=u.ProjectFeatures.Select(o=>o.Title).Take(4).ToList()}).ToListAsync();
            
            var result = new ResFilterWorkSampleDto();
            result.SetPaging(pager);
            return result.SetWorkSamples(list);
    }

    public async Task<WorkSampleDto> GetWorkSample(long WorkSampleId)
    {
        //get worksample
        var WorkSample=await _WorkSampleRepository.GetEntity(WorkSampleId);
        if(WorkSample is not null)
        {   var worksampleDto= new WorkSampleDto(){
            Content=WorkSample.Content,
            CustomUrl=WorkSample.CustomUrl,
            EmployerName=WorkSample.EmployerName,
            HeaderImagePath=PathTools.GetWorkSampleImages+WorkSample.HeaderImageName,
            LogoImagePath=PathTools.GetWorkSampleImages+WorkSample.LogoImageName,
            ProjectName=WorkSample.ProjectName,
            ShortDescription=WorkSample.ShortDescription,
            ProjectFeatures=new List<string>(),
            DesignedPages=new List<string>(),
            ImageGalleryPaths=new List<string>()
        };
            //get imageGallery
            var ImageGallary=await _ImageGalleryService.GetActiveGalleryByWorkSampleId(WorkSampleId);
            if(ImageGallary.Any())
            {
                foreach (var Image in ImageGallary)
                {
                    worksampleDto.ImageGalleryPaths.Add(Image.ImagePath);
                }
            }
            //get likes
            int Like= _LikeRepository.GetAllEntity().Count(l=>l.WorkSampleId==WorkSampleId&&l.IsRemove==false);
            worksampleDto.Likes=Like>0?Like:0;
            //get project features
            var ProjectFeatures=await _ProjectFeatureRepository.GetAllEntity().Where(p=>p.FeatureType==WorkSampleFeatureType.ProjectFeatured && p.WorkSampleId==WorkSampleId && p.IsRemove==false).ToListAsync();

            if(ProjectFeatures.Any())
            {
                foreach (var Feature in ProjectFeatures)
                {
                    worksampleDto.ProjectFeatures.Add(Feature.Title);
                }
            }
            //get Designed pages
            var DesignedPages=await _ProjectFeatureRepository.GetAllEntity().Where(p=>p.FeatureType==WorkSampleFeatureType.DesignedPages && p.WorkSampleId==WorkSampleId && p.IsRemove==false).ToListAsync();

            if(DesignedPages.Any())
            {
                foreach (var Pages in DesignedPages)
                {
                    worksampleDto.DesignedPages.Add(Pages.Title);
                }
            }

            return worksampleDto;
        }
        return null;
        
    }
}
