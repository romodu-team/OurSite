using OurSite.Core.DTOs.WorkSampleDtos;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Utilities;
using OurSite.DataLayer.Entities.WorkSamples;
using OurSite.DataLayer.Interfaces;

namespace OurSite.Core.Services.Repositories;

public class WorkSampleService : IWorkSampleService
{
    private IGenericReopsitories<WorkSample> _WorkSampleRepository;
    private IGenericReopsitories<ProjectFeatures> _ProjectFeatureRepository;
    public WorkSampleService(IGenericReopsitories<WorkSample> WorkSampleRepository,IGenericReopsitories<ProjectFeatures> ProjectFeatureRepository)
    {
        _ProjectFeatureRepository=ProjectFeatureRepository;
        _WorkSampleRepository=WorkSampleRepository;
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
        {
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

            if(request.HeaderImageName is not null){
                
            }
           // return worksample.Id; not correct
        }
        catch (System.Exception)
        {
            
            return -1;
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

    public Task<List<WorkSampleDto>> GetAllWorkSamples()
    {
        throw new NotImplementedException();
    }

    public Task<WorkSampleDto> GetWorkSample(long ProjectId)
    {
        throw new NotImplementedException();
    }
}
