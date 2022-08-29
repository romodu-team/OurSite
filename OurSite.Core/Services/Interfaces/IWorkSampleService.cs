using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OurSite.Core.DTOs.WorkSampleDtos;
using OurSite.DataLayer.Entities.RatingModel;

namespace OurSite.Core.Services.Interfaces
{
    public interface IWorkSampleService : IDisposable
    {
        Task<ResCreateWorkSampleDto> CreateWorkSample(CreateWorkSampleDto request);
        Task<ResFilterWorkSampleDto> GetAllWorkSamples(ReqFilterWorkSampleDto request);
        Task<WorkSampleDto> GetWorkSample(long WorkSampleId);
        Task<ResWorkSample> DeleteWorkSample(long WorkSampleId);

        Task<ResWorkSample> EditWorkSample(long worksampleId,EditWorkSampleDto  request);
        Task<ResLike> AddLike(string userIp, long workSampleId);
    }
}
