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
        Task<List<WorkSampleDto>> GetAllWorkSamples();
        Task<WorkSampleDto> GetWorkSample(long ProjectId);
        Task<ResWorkSample> DeleteWorkSample(DeleteWorkSampleDto  request);

        Task<ResWorkSample> EditWorkSample(EditWorkSampleDto  request);
    }
}
