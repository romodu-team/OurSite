using Microsoft.EntityFrameworkCore;
using OurSite.Core.DTOs.ConsultationRequestDtos;
using OurSite.Core.DTOs.MailDtos;
using OurSite.Core.DTOs.NotificationDtos;
using OurSite.Core.DTOs.Paging;
using OurSite.Core.DTOs.UserDtos;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Services.Interfaces.Mail;
using OurSite.Core.Utilities.Extentions.Paging;
using OurSite.DataLayer.Entities.ConsultationRequest;
using OurSite.DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Services.Repositories.Forms
{
    public class ConsultationRequestService : IConsultationRequestService
    {
        #region constructor
        private readonly IGenericRepository<ConsultationRequest> consultationRequestRepo;
        private readonly IGenericRepository<CheckBoxs> checkBoxsRepo;
        private readonly IGenericRepository<ItemSelected> itemSelectedRepo;
        private readonly INotificationService _notificationService;
        private IAdminService _AdminService;
        private readonly IMailService _mailService;

        public ConsultationRequestService(IMailService mailService, IAdminService AdminService, INotificationService notificationService, IGenericRepository<ItemSelected> itemSelectedRepo, IGenericRepository<CheckBoxs> checkBoxsRepo, IGenericRepository<ConsultationRequest> consultationRequestRepo)
        {
            _mailService = mailService;
            this.consultationRequestRepo = consultationRequestRepo;
            this.checkBoxsRepo = checkBoxsRepo;
            this.itemSelectedRepo = itemSelectedRepo;
            _notificationService = notificationService;
            _AdminService = AdminService;
        }


        #region Dispose
        public void Dispose()
        {
            consultationRequestRepo.Dispose();
            checkBoxsRepo.Dispose();
            itemSelectedRepo.Dispose();
            _notificationService.Dispose();

        }


        #endregion
        #endregion


        public async Task<bool> SendConsultationForm(ConsultationRequestDto consultationRequestDto, string? SubmittedFileName)
        {
            bool flag = true;
            ConsultationRequest request = new ConsultationRequest()
            {
                FirstName = consultationRequestDto.FirstName,
                LastName = consultationRequestDto.LastName,
                UserEmail = consultationRequestDto.Email,
                UserPhoneNumber = consultationRequestDto.PhoneNumber,
                Content = consultationRequestDto.Content,
                SubmittedFileName = SubmittedFileName,
                IsRead = false
            };
            try
            {
                await consultationRequestRepo.AddEntity(request);
                await consultationRequestRepo.SaveEntity();
            }
            catch (Exception)
            {

                flag = false;
                return flag;
            }

            if (consultationRequestDto.ItemSelectedsId is not null && consultationRequestDto.ItemSelectedsId.Count > 0)
            {
                var list = consultationRequestDto.ItemSelectedsId[0].Split(",");
                foreach (var item in list)
                {
                    var checkbox = await checkBoxsRepo.GetEntity(Convert.ToInt64(item));
                    if (checkbox != null)
                    {
                        var selectedItem = new ItemSelected()
                        {
                            CheckBoxId = Convert.ToInt64(item),
                            ConsultationFormId = request.Id

                        };
                        await itemSelectedRepo.AddEntity(selectedItem);
                    }

                }
                try
                {
                    await itemSelectedRepo.SaveEntity();

                    flag = true;
                    //send notification to admins
                    var Adminaccounts = await _AdminService.GetAllAdmin(new ReqFilterUserDto { PageId = 1, TakeEntity = 1000 });
                    if (Adminaccounts.Admins != null && Adminaccounts.Admins.Count > 0)
                    {
                        foreach (var admin in Adminaccounts.Admins)
                        {
                            await _notificationService.CreateNotification(new ReqCreateNotificationDto { AccountUUID = admin.UUID, Message = $"فرم درخواست مشاوره جدید دریافت شد." });
                        }
                    }
                    //send email to support
                    var MailMessage = new MailRequestDto
                    {
                        ToEmail = "Support@romodu.com",
                        Subject = "پیام تماس با ما جدید دریافت شد",
                        Body = $"متن پیام : فرم درخواست مشاوره جدید دریافت شد."
                    };
                    await _mailService.SendEmailAsync(MailMessage);
                    return flag;
                }
                catch
                {
                    flag = false;
                    return flag;
                }
            }
            //send notification to admins
            var accounts = await _AdminService.GetAllAdmin(new ReqFilterUserDto { PageId = 1, TakeEntity = 1000 });
            if (accounts.Admins != null && accounts.Admins.Count > 0)
            {
                foreach (var admin in accounts.Admins)
                {
                    await _notificationService.CreateNotification(new ReqCreateNotificationDto { AccountUUID = admin.UUID, Message = $"فرم درخواست مشاوره جدید دریافت شد." });
                }
            }
            //send email to support
            var MailMessageDto = new MailRequestDto
            {
                ToEmail = "Support@romodu.com",
                Subject = "پیام تماس با ما جدید دریافت شد",
                Body = $"متن پیام : فرم درخواست مشاوره جدید دریافت شد."
            };
            await _mailService.SendEmailAsync(MailMessageDto);
            return flag;
        }



        #region See All ConsultationRequest Form

        public async Task<ResFilterConsultationRequestDto> GetAllConsultationRequest(ReqFilterConsultationRequestDto filter)
        {

            var consultationRequestQuery = consultationRequestRepo.GetAllEntity();
            var count = (int)Math.Ceiling(consultationRequestQuery.Count() / (double)filter.TakeEntity);
            var pager = Pager.Build(count, filter.PageId, filter.TakeEntity);
            var list = await consultationRequestRepo.GetAllEntity().Paging(pager).Where(u => u.IsRemove == false).Select(x => new GetAllConsultationRequestDto { IsRead = x.IsRead, Id = x.Id, FirstName = x.FirstName,LastName=x.LastName, Email = x.UserEmail, PhoneNumber = x.UserPhoneNumber }).ToListAsync();
            var result = new ResFilterConsultationRequestDto();
            result.SetPaging(pager);
            return result.SetConsultationRequests(list);

        }

        #endregion

        #region Get Consulation From
        public async Task<GetConsulationFormDto> GetConsulationForm(long ConsultationFormId)
        {
            var consulationFrom = await consultationRequestRepo.GetAllEntity().Include(c => c.ItemSelecteds).ThenInclude(c => c.CheckBox).SingleOrDefaultAsync(c => c.Id == ConsultationFormId);
            if (consulationFrom is not null)
            {
                var dto = new GetConsulationFormDto()
                {
                    Content = consulationFrom.Content,
                    CreateDate = consulationFrom.CreateDate.ToString(),
                    LastUpdateDate = consulationFrom.LastUpdate.ToString(),
                    Email = consulationFrom.UserEmail,
                    FirstName = consulationFrom.FirstName,
                    LastName=consulationFrom.LastName,
                    PhoneNumber = consulationFrom.UserPhoneNumber,
                    ItemSelected = new List<DTOs.CheckBoxDtos.CheckBoxDto?>(),
                    IsRead = consulationFrom.IsRead
                };
                foreach (var item in consulationFrom.ItemSelecteds)
                {
                    var checkbox = new DTOs.CheckBoxDtos.CheckBoxDto { Description = item.CheckBox.Description, IconName = item.CheckBox.IconName, Title = item.CheckBox.CheckBoxName, Id = item.CheckBox.Id };
                    dto.ItemSelected.Add(checkbox);
                }

                return dto;
            }
            return null;
        }
        #endregion
        public async Task<bool> ChangeReadStatus(long ConsultationFormId)
        {
            var form = await consultationRequestRepo.GetEntity(ConsultationFormId);
            form.IsRead = !form.IsRead;
            try
            {
                consultationRequestRepo.UpDateEntity(form);
                await consultationRequestRepo.SaveEntity();
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }

        }

    }
}
