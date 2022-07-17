using System;
using OurSite.Core.DTOs.ProjectDtos;
using OurSite.Core.Services.Interfaces.Projects;
using OurSite.DataLayer.Entities.Projects;
using OurSite.DataLayer.Interfaces;
using static OurSite.Core.DTOs.ProjectDtos.CreatProjectDto;

namespace OurSite.Core.Services.Repositories.Forms
{
    public class ProjectService : IProject
    {
        private IGenericReopsitories<Project> ProjectsRepository;
        public ProjectService(IGenericReopsitories<Project> ProjectsRepository)
        {
            this.ProjectsRepository = ProjectsRepository;
        }


        public async Task<ResProject> CreatProject(CreatProjectDto prodto,long userId)
        {
            if (!string.IsNullOrWhiteSpace(prodto.Name) && !string.IsNullOrWhiteSpace(prodto.Description) && )
            {
                Project newPro = new Project()
                {
                    IsRemove = false,
                    Name = prodto.Name,
                    Type = prodto.Type,
                    Situation = prodto.Situation,
                    UserId = userId,
                    Description = prodto.Description,
                    PlanDetails = prodto.PlanDetails
                };

                try
                {
                    await ProjectsRepository.AddEntity(newPro);
                    await ProjectsRepository.SaveEntity();
                    return ResProject.Success;
                }
                catch (Exception ex)
                {
                    return ResProject.Faild; //error in save
                }   
            }
            return ResProject.InvalidInput;
         }

        public Task<bool> DeleteProject(long ProjectId)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<ResProject> EditProject(EditProjectDto prodto)
        {
            throw new NotImplementedException();
        }

        public Task<Project> GetAllProject()
        {
            throw new NotImplementedException();
        }

        public Task<Project> GetProject(long ProjectId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UploadContract(ReqUploadContractDto profiledto)
        {
            throw new NotImplementedException();
        }
    }
}

