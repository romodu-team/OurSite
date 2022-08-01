using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OurSite.Core.DTOs.WorkSampleDtos;

namespace OurSite.Core.Services.Interfaces
{
    public interface IWorkSampleService
    {
        Task<ResCreateWorkSampleDto> CreateWorkSample(CreateWorkSampleDto request);
        Task<ResFilterWorkSampleDto> GetAllWorkSamples(ReqFilterWorkSampleDto request);
        Task<WorkSampleDto> GetWorkSample(long WorkSampleId);
        Task<ResWorkSample> DeleteWorkSample(long WorkSampleId);

        Task<ResWorkSample> EditWorkSample(long worksampleId,EditWorkSampleDto  request);
    }
}
