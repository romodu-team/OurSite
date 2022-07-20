using System;
using OurSite.Core.DTOs.ProjectDtos;
using OurSite.DataLayer.Entities.Projects;
using static OurSite.Core.DTOs.ProjectDtos.CreatProjectDto;

namespace OurSite.Core.Services.Interfaces.Projecta
{
    public interface IProject : IDisposable
    {
        Task<Project> GetAllProject();
        Task<Project> GetProject(long ProjectId);
        Task<ResProject> CreateProject(CreatProjectDto prodto, long userId);
        Task<resUploadContract> UploadContract(ReqUploadContractDto reqUploadContract);
        Task<ResProject> DeleteProject(DeleteProjectDto ReqDeleteProject);

        #region Admin
        Task<ResProject> EditProject(EditProjectDto prodto);
        #endregion
    }
}

