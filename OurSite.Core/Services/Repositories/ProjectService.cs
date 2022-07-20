using System;
using Microsoft.EntityFrameworkCore;
using OurSite.Core.DTOs.ProjectDtos;
using OurSite.Core.Services.Interfaces.Projecta;
using OurSite.DataLayer.Entities.Projects;
using OurSite.DataLayer.Interfaces;
using static OurSite.Core.DTOs.ProjectDtos.CreatProjectDto;

namespace OurSite.Core.Services.Repositories
{
    public class ProjectService : IProject
    {
        #region Cons&Dis
        private IGenericReopsitories<Project> ProjectsRepository;
        public ProjectService(IGenericReopsitories<Project> ProjectsRepository)
        {
            this.ProjectsRepository = ProjectsRepository;
        }

        public void Dispose()
        {
            ProjectsRepository.Dispose();
        }
        #endregion

        #region Creat project
        public async Task<ResProject> CreateProject(CreatProjectDto prodto, long userId)
        {
            if (!string.IsNullOrWhiteSpace(prodto.Name) && !string.IsNullOrWhiteSpace(prodto.Description))
            {
                Project newPro = new Project()
                {
                    IsRemove = false,
                    Name = prodto.Name,
                    Type = prodto.Type,
                    Situation = situations.Pending,
                    UserId = userId,
                    Description = prodto.Description,
                    PlanDetails = prodto.PlanDetails,

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
        #endregion

        #region Delete project
        public async Task<ResProject> DeleteProject(DeleteProjectDto ReqDeleteProject)
        {
            var project = await ProjectsRepository.GetEntity(ReqDeleteProject.ProId);
            if(project is not null)
            {
                if(ReqDeleteProject.AdminId is not null && ReqDeleteProject.UserId is null)
                {
                    var IsRemove = await ProjectsRepository.DeleteEntity(project.Id);
                    if (IsRemove is true)
                    {
                        await ProjectsRepository.SaveEntity();
                        return ResProject.Success;
                    }
                    else
                    {
                        return ResProject.Error;

                    }
                }

                else
                {
                    if (project.Situation == situations.Pending)
                    {
                        var IsRemove = await ProjectsRepository.DeleteEntity(project.Id);
                        if (IsRemove is true)
                        {
                            await ProjectsRepository.SaveEntity();
                            return ResProject.Success;
                        }
                        else
                        {
                            return ResProject.Error;

                        }
                    }
                    return ResProject.SitutionError;

                }
            }

            return ResProject.NotFound;
            
        }
        #endregion

        #region Update project
        public async Task<ResProject> EditProject(EditProjectDto prodto)
        {
            var res = await ProjectsRepository.GetAllEntity().AnyAsync(x => x.Id == prodto.ProId);
            if (res is true)
            {

                var pro = await ProjectsRepository.GetEntity(prodto.ProId);
                if (!string.IsNullOrWhiteSpace(prodto.Name))
                    pro.Name = prodto.Name;
                if (prodto.Type is not null)
                    pro.Type = (ProType)prodto.Type;
                if (prodto.StartTime is not null)
                    pro.StartTime = Convert.ToDateTime(prodto.StartTime);
                if (prodto.EndTime is not null)
                    pro.EndTime = Convert.ToDateTime(prodto.EndTime);
                if (prodto.Price is null)
                    pro.Price = prodto.Price;
                if (!string.IsNullOrWhiteSpace(prodto.Description))
                    pro.Description = prodto.Description;
                if (prodto.Situation is not null)
                    pro.Situation = (situations)prodto.Situation;
                if (!string.IsNullOrWhiteSpace(prodto.PlanDetails))
                    pro.PlanDetails = prodto.PlanDetails;
                if (prodto.AdminId is null)
                    pro.AdminId = prodto.AdminId;
                if (!string.IsNullOrWhiteSpace(prodto.ContractFileName))
                    pro.ContractFileName = prodto.ContractFileName;
                try
                {
                    ProjectsRepository.UpDateEntity(pro);
                    await ProjectsRepository.SaveEntity();
                    return ResProject.Success;
                }
                catch (Exception ex)
                {
                    return ResProject.Faild;
                }


            }
            return ResProject.NotFound;

        }
        #endregion


        public Task<Project> GetAllProject()
        {
            throw new NotImplementedException();
        }

        public async Task<GetProjectDto> GetProject(long ProjectId)
        {
            var project = await ProjectsRepository.GetEntity(ProjectId);
            if (project is not null)
            {
                GetProjectDto ViewrProject = new GetProjectDto()
                {
                    Name = project.Name,
                    Type = project.Type,
                    StartTime = project.StartTime,
                    EndTime = project.EndTime,
                    Price = project.Price,
                    Description = project.Description,
                    Situation = project.Situation,
                    PlanDetails = project.PlanDetails,
                    ContractFileName = project.ContractFileName
                };
                return ViewrProject;
            }
            return null;
        }

        public Task<bool> UploadContract(ReqUploadContractDto profiledto)
        {
            throw new NotImplementedException();
        }
    }
}

