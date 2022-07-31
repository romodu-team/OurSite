using System;
using OurSite.Core.DTOs.ProjectDtos;
using OurSite.Core.DTOs.UserDtos;
using OurSite.DataLayer.Entities.Projects;
using static OurSite.Core.DTOs.ProjectDtos.CreatProjectDto;

namespace OurSite.Core.Services.Interfaces.Projecta
{
    public interface IProject : IDisposable
    {
        Task<ResFilterProjectDto> GetAllProject(ReqFilterProjectDto filter);
        Task<GetProjectDto> GetProject(long ProjectId);
        Task<ResProject> CreateProject(CreatProjectDto prodto, long userId);
        Task<ResProject> DeleteProject(DeleteProjectDto ReqDeleteProject,bool IsAdmin);

        #region Admin
        Task<ResProject> EditProject(EditProjectDto prodto);
        Task<resUploadContract> UploadContract(ReqUploadContractDto reqUploadContract);
        #endregion
    }
}

