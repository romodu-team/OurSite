﻿using System;
using Microsoft.EntityFrameworkCore;
using OurSite.Core.DTOs.ProjectDtos;
using OurSite.Core.DTOs.UserDtos;
using OurSite.Core.Services.Interfaces.Projecta;
using OurSite.Core.Utilities;
using OurSite.DataLayer.Entities.ConsultationRequest;
using OurSite.DataLayer.Entities.Projects;
using OurSite.DataLayer.Interfaces;
using static OurSite.Core.DTOs.ProjectDtos.CreatProjectDto;

namespace OurSite.Core.Services.Repositories
{
    public class ProjectService : IProject
    {
        #region Cons&Dis
        private IGenericReopsitories<Project> ProjectsRepository;
        private IGenericReopsitories<CheckBoxs> CheckBoxRepository;
        private IGenericReopsitories<SelectedProjectPlan> SelectedProjectRepository;
        public ProjectService(IGenericReopsitories<SelectedProjectPlan> SelectedProjectRepository,IGenericReopsitories<Project> ProjectsRepository,IGenericReopsitories<CheckBoxs> CheckBoxRepository)
        {
            this.ProjectsRepository = ProjectsRepository;
            this.CheckBoxRepository=CheckBoxRepository;
            this.SelectedProjectRepository=SelectedProjectRepository;
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
                    Description = prodto.Description

                };

                try
                {
                    await ProjectsRepository.AddEntity(newPro);
                    await ProjectsRepository.SaveEntity();
                    if(prodto.PlanDetails is not null)
                    {
                        foreach (var PlanItem in prodto.PlanDetails)
                        {
                            //if plan is exist
                            var plan= await CheckBoxRepository.GetEntity(PlanItem);
                            if(plan is not null){
                                 //add plan to selected plan
                                SelectedProjectPlan selectedPlan= new SelectedProjectPlan(){
                                    CheckBoxId=PlanItem,
                                    ProjectId=newPro.Id
                                };
                                await SelectedProjectRepository.AddEntity(selectedPlan);
                                
                            }
                           
                        }
                        await SelectedProjectRepository.SaveEntity();
                    }
                   
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
                    //add selected project plan to database
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

        #region list project
        public Task<ResFilterProjectDto> GetAllProject(ReqFilterProjectDto filter)
        {
            var ProjectsQuery = ProjectsRepository.GetAllEntity().AsQueryable();
            switch (filter.CreatDateOrderBy)
            {
                case ProjectsCreatDateOrderBy.CreateDateAsc:
                    ProjectsQuery = ProjectsQuery.OrderBy(u => u.CreateDate);
                    break;
                case ProjectsCreatDateOrderBy.CreateDateDec:
                    ProjectsQuery = ProjectsQuery.OrderByDescending(u => u.CreateDate);
                    break;
                default:
                    break;
            }

            switch (filter.TypeOrderBy)
            {
                case ProType.WebDesign:
                    ProjectsQuery = ProjectsRepository.GetAllEntity().Where(x => x.Type == ProType.WebDesign);
                    break;
                case ProType.seo:
                    ProjectsQuery = ProjectsRepository.GetAllEntity().Where(x => x.Type == ProType.seo);
                    break;
                case ProType.Graphic:
                    ProjectsQuery = ProjectsRepository.GetAllEntity().Where(x => x.Type == ProType.Graphic);
                    break;
                case ProType.Cms:
                    ProjectsQuery = ProjectsRepository.GetAllEntity().Where(x => x.Type == ProType.Cms);
                    break;
                case ProType.Appliction:
                    ProjectsQuery = ProjectsRepository.GetAllEntity().Where(x => x.Type == ProType.Appliction);
                    break;
                case ProType.All:
                    ProjectsQuery = ProjectsRepository.GetAllEntity();
                    break;
                default:
                    break;
            }

            switch (filter.RemoveFilter)
            {
                case ProjectRemoveFilter.Deleted:
                    ProjectsQuery = ProjectsQuery.Where(x => x.IsRemove == true);
                    break;
                case ProjectRemoveFilter.NotDeleted:
                    ProjectsQuery = ProjectsQuery.Where(x => x.IsRemove == false);
                    break;
                case ProjectRemoveFilter.All:
                    ProjectsQuery = ProjectsRepository.GetAllEntity();
                    break;
                default:
                    break;
            }
            switch (filter.SituationsFilter)
            {
                case situations.Cancel:
                    ProjectsQuery = ProjectsRepository.GetAllEntity().Where(x => x.Situation == situations.Cancel);
                    break;
                case situations.Doing:
                    ProjectsQuery = ProjectsRepository.GetAllEntity().Where(x => x.Situation == situations.Doing);
                    break;
                case situations.AwatingPayment:
                    ProjectsQuery = ProjectsRepository.GetAllEntity().Where(x => x.Situation == situations.AwatingPayment);
                    break;
                case situations.Pending:
                    ProjectsQuery = ProjectsRepository.GetAllEntity().Where(x => x.Situation == situations.Pending);
                    break;
                case situations.End:
                    ProjectsQuery = ProjectsRepository.GetAllEntity().Where(x => x.Situation == situations.End);
                    break;
                case situations.All:
                    ProjectsQuery = ProjectsRepository.GetAllEntity();
                    break;
                default:
                    break;
            }

            if(!string.IsNullOrWhiteSpace())








        }
        #endregion

        #region Get project
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
                    ContractFileName = project.ContractFileName
                };
                var selectedPlans = await SelectedProjectRepository.GetAllEntity().Where(x => x.ProjectId == ProjectId).Include(x => x.CheckBox).ToListAsync();
                foreach (var item in selectedPlans)
                {
                    ViewrProject.PlanDetails.Add(item.CheckBox);
                }
                return ViewrProject;
            }
            return null;
        }
        #endregion

        #region Contract Manage
        public async Task<resUploadContract> UploadContract(ReqUploadContractDto reqUploadContract)
        {
            // check if project exist
            var Project = await ProjectsRepository.GetEntity(reqUploadContract.ProjectId);
            if (Project is null)
                return resUploadContract.projectNotFound;
            //if file not null
            if (reqUploadContract.ContractFile is null)
                return resUploadContract.FileNotFound;
            //upload file and save file name in database
            var resUpload = await FileUploader.UploadFile(PathTools.ContractUploadPath, reqUploadContract.ContractFile, 10);
            switch (resUpload.Status)
            {
                case resFileUploader.Success:
                    //save filename in database
                    Project.ContractFileName = resUpload.FileName;
                    try
                    {
                        ProjectsRepository.UpDateEntity(Project);
                        await ProjectsRepository.SaveEntity();
                        return resUploadContract.Success;
                    }
                    catch (System.Exception)
                    {

                        return resUploadContract.Error;
                    }


                case resFileUploader.NoContent:
                    return resUploadContract.FileNotFound;
                case resFileUploader.ToBig:
                    return resUploadContract.TooBig;
                case resFileUploader.InvalidExtention:
                    return resUploadContract.FileExtentionError;
                case resFileUploader.Failure:
                    return resUploadContract.Error;
                default:
                    return resUploadContract.Error;
            }
        }

        #endregion

    }
}

