using Microsoft.VisualBasic.CompilerServices;
using System;
using Microsoft.EntityFrameworkCore;
using OurSite.Core.DTOs.Paging;
using OurSite.Core.DTOs.CheckBoxDtos;
using OurSite.Core.DTOs.ProjectDtos;
using OurSite.Core.DTOs.UserDtos;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Services.Interfaces.Projecta;
using OurSite.Core.Utilities;
using OurSite.Core.Utilities.Extentions.Paging;
using OurSite.DataLayer.Entities.ConsultationRequest;
using OurSite.DataLayer.Entities.Projects;
using OurSite.DataLayer.Interfaces;
using static OurSite.Core.DTOs.ProjectDtos.CreateProjectDto;
using OurSite.Core.Services.Interfaces.Mail;
using OurSite.Core.DTOs.NotificationDtos;
using OurSite.Core.DTOs.MailDtos;
using OurSite.Core.Utilities.Extentions;

namespace OurSite.Core.Services.Repositories
{
    public class ProjectService : IProject
    {
        #region Cons&Dis
        private IGenericRepository<Project> ProjectsRepository;
        private IGenericRepository<CheckBoxs> CheckBoxRepository;
        private IGenericRepository<SelectedProjectPlan> SelectedProjectRepository;
        private IUserService UserService;
        private INotificationService _notificationService;
        private IMailService _mailService;
        public ProjectService(IMailService mailService,INotificationService notificationService,IGenericRepository<SelectedProjectPlan> SelectedProjectRepository,IGenericRepository<Project> ProjectsRepository,IGenericRepository<CheckBoxs> CheckBoxRepository, IUserService userService)
        {
            this.ProjectsRepository = ProjectsRepository;
            this.CheckBoxRepository=CheckBoxRepository;
            this.SelectedProjectRepository=SelectedProjectRepository;
            this.UserService = userService;
            _notificationService=notificationService;
            _mailService=mailService;
        }

        public void Dispose()
        {
            ProjectsRepository.Dispose();
            CheckBoxRepository.Dispose();
            SelectedProjectRepository.Dispose();
            UserService.Dispose();
            CheckBoxService.Dispose();
        }
        #endregion

        #region Creat project
        public async Task<ResProject> CreateProject(CreateProjectDto prodto, long userId)
        {
            var user = await UserService.ViewUser(userId);
            if (user is not null)
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
                        if (prodto.PlanDetails is not null)
                        {
                            foreach (var PlanItem in prodto.PlanDetails)
                            {
                                //if plan is exist
                                var plan = await CheckBoxRepository.GetEntity(PlanItem);
                                if (plan is not null)
                                {
                                    //add plan to selected plan
                                    SelectedProjectPlan selectedPlan = new SelectedProjectPlan()
                                    {
                                        CheckBoxId = PlanItem,
                                        ProjectId = newPro.Id
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
            return ResProject.NotFound;
        }
        #endregion

        #region Delete project
        public async Task<ResProject> DeleteProject(long ProId, bool IsAdmin)
        {
            var project = await ProjectsRepository.GetAllEntity().Where(x => x.Id == ProId).Include(x => x.selectedProjectPlans).SingleOrDefaultAsync();
            if(project is not null)
            {
                if(IsAdmin)
                {
                    foreach(var item in project.selectedProjectPlans)
                    {
                        var RemovePlanProject = await SelectedProjectRepository.DeleteEntity(item.Id);
                    }
                    
                    var IsRemove = await ProjectsRepository.DeleteEntity(project.Id);
                    if (IsRemove is true)
                    {
                        await SelectedProjectRepository.SaveEntity();
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
                        foreach (var item in project.selectedProjectPlans)
                        {
                            var RemovePlanProject = await SelectedProjectRepository.DeleteEntity(item.Id);
                        }
                        var IsRemove = await ProjectsRepository.DeleteEntity(project.Id);
                        if (IsRemove is true)
                        {
                            await SelectedProjectRepository.SaveEntity();
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
                if (prodto.PlanDetails is not null)
                    await CheckBoxService.ChangeProjectPlans(prodto.ProId , prodto.PlanDetails);
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
        public async Task<ResFilterProjectDto> GetAllProject(ReqFilterProjectDto filter)
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
                    ProjectsQuery = ProjectsQuery.Where(x => x.Type == ProType.WebDesign);
                    break;
                case ProType.seo:
                    ProjectsQuery = ProjectsQuery.Where(x => x.Type == ProType.seo);
                    break;
                case ProType.Graphic:
                    ProjectsQuery = ProjectsQuery.Where(x => x.Type == ProType.Graphic);
                    break;
                case ProType.Cms:
                    ProjectsQuery = ProjectsQuery.Where(x => x.Type == ProType.Cms);
                    break;
                case ProType.Appliction:
                    ProjectsQuery = ProjectsQuery.Where(x => x.Type == ProType.Appliction);
                    break;
                case ProType.All:
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
                    break;
                default:
                    break;
            }
            switch (filter.SituationsFilter)
            {
                case situations.Cancel:
                    ProjectsQuery = ProjectsQuery.Where(x => x.Situation == situations.Cancel);
                    break;
                case situations.Doing:
                    ProjectsQuery = ProjectsQuery.Where(x => x.Situation == situations.Doing);
                    break;
                case situations.AwatingPayment:
                    ProjectsQuery = ProjectsQuery.Where(x => x.Situation == situations.AwatingPayment);
                    break;
                case situations.Pending:
                    ProjectsQuery = ProjectsQuery.Where(x => x.Situation == situations.Pending);
                    break;
                case situations.End:
                    ProjectsQuery = ProjectsQuery.Where(x => x.Situation == situations.End);
                    break;
                case situations.All:
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrWhiteSpace(filter.UserName))
                ProjectsQuery = ProjectsQuery.Include(x => x.User).Where(x => x.User.UserName.Contains(filter.UserName.ToLower()));

            var count = (int)Math.Ceiling(ProjectsQuery.Count() / (double)filter.TakeEntity);
            var pager = Pager.Build(count, filter.PageId, filter.TakeEntity);
            var list = await ProjectsQuery.Paging(pager).Include(x => x.User).Select(x => new GetAllProjectDto { CreatDate = x.CreateDate.PersianDate(), ProjectId = x.Id, Situations = x.Situation, IsRemove = x.IsRemove, Type = x.Type, UserName = x.User.UserName }).ToListAsync();


            var result = new ResFilterProjectDto();
            result.SetPaging(pager);
            return result.SetProject(list);

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
                    ProId = ProjectId,
                    Name = project.Name,
                    Type = project.Type,
                    StartTime = project.StartTime != null? project.StartTime.Value.PersianDate():null,
                    EndTime = project.EndTime != null? project.EndTime.Value.PersianDate():null,
                    Price = project.Price,
                    Description = project.Description,
                    Situation = project.Situation,
                    ContractFilePath = PathTools.ContractUploadName + project.ContractFileName,
                    PlanDetails=new List<CheckBoxDto>()
                };
                var selectedPlans = await SelectedProjectRepository.GetAllEntity().Where(x => x.ProjectId == ProjectId).Include(x => x.CheckBox).ToListAsync();
                foreach (var item in selectedPlans)
                {
                    ViewrProject.PlanDetails.Add(new CheckBoxDto{Id=item.CheckBox.Id,Description=item.CheckBox.Description,IconName=item.CheckBox.IconName,Title=item.CheckBox.CheckBoxName});
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
                    if (Project.ContractFileName != null)
                        FileUploader.DeleteFile(PathTools.ContractUploadPath + "\\" + Project.ContractFileName);
                    Project.ContractFileName = resUpload.FileName;
                    try
                    {
                        ProjectsRepository.UpDateEntity(Project);
                        await ProjectsRepository.SaveEntity();
                        //send notification and email to User
                        var user =await UserService.GetUserById(Project.UserId);
                        await _notificationService.CreateNotification(new ReqCreateNotificationDto{AccountUUID=user.UUID,Message=$"قرارداد جدید برای پروژه شماره  {Project.Id} ثبت شد."});
                        if(user.Email != null)
                            await _mailService.SendEmailAsync(new MailRequestDto{ToEmail=user.Email,Subject=$"قرارداد جدید برای پروژه شماره  {Project.Id} ثبت شد.",Body="متن اپلود قرارداد",Attachments=null});
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

        public async Task<string> ReturnContract(long ProjectId)
        {
            var ContractName = await ProjectsRepository.GetAllEntity().Where(x => x.Id == ProjectId).Select(x => x.ContractFileName).SingleOrDefaultAsync();
            if (ContractName is not null)
            {
                return String.Concat(PathTools.ContractUploadName, ContractName);
            }
            return null;
        }

        #endregion

    }
}

