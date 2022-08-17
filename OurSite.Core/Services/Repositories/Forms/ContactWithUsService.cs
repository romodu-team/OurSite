using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using OurSite.Core.DTOs.ContactWithUsDtos;
using OurSite.Core.DTOs.MailDtos;
using OurSite.Core.DTOs.Paging;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Services.Interfaces.Mail;
using OurSite.Core.Utilities.Extentions.Paging;
using OurSite.DataLayer.Entities.ContactWithUs;
using OurSite.DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OurSite.Core.DTOs.NotificationDtos;
using OurSite.Core.DTOs.UserDtos;

namespace OurSite.Core.Services.Repositories.Forms
{
    public class ContactWithUsService : IContactWithUsService
    {
        #region constructor
        private readonly IGenericRepository<ContactWithUs> contactWithUsRepo;
        private readonly IMailService mailService;
        private readonly INotificationService _notificationService;
        private IAdminService _AdminService;
        public ContactWithUsService(IAdminService AdminService,INotificationService notificationService,IGenericRepository<ContactWithUs> contactWithUsRepo,IMailService mailService)
        {
            this.contactWithUsRepo = contactWithUsRepo;
            this.mailService=mailService;
            _notificationService=notificationService;
            _AdminService=AdminService;
        }
        
        #region Dispose
        public void Dispose()
        {
            contactWithUsRepo.Dispose();
            _notificationService.Dispose();
        }
        #endregion
        #endregion

        #region Send ContactWithUs Form
        public async Task<bool> SendContactForm(ContactWithUsDto contactWithUsDto)
        {
            ContactWithUs form1 = new ContactWithUs()
            {
                UserFirstName = contactWithUsDto.UserFirstName,
                UserLastName = contactWithUsDto.UserLastName,
                UserEmail = contactWithUsDto.UserEmail,
                UserPhoneNumber = contactWithUsDto.UserPhoneNumber,
                Expration = contactWithUsDto.Expration,
            };
            try
            {
                await contactWithUsRepo.AddEntity(form1);
                await contactWithUsRepo.SaveEntity();
                //send email for admin:you have new massage
                var MailMessageDto= new MailRequestDto{
                    ToEmail="Support@romodu.com",
                    Subject="پیام تماس با ما جدید دریافت شد",
                    Body=$"متن پیام : {form1.Expration}"};
                await mailService.SendEmailAsync(MailMessageDto);
                //send notification to admins
                var accounts = await _AdminService.GetAllAdmin(new ReqFilterUserDto { PageId=1,TakeEntity=1000});
                if(accounts.Admins!=null && accounts.Admins.Count > 0)
                {
                    foreach (var admin in accounts.Admins)
                    {
                        await _notificationService.CreateNotification(new ReqCreateNotificationDto { AccountUUID = admin.UUID, Message = $"پیام تماس با ما جدید دریافت شد." });
                    }
                }
       
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region See All ContactWithUs Form

        public async Task<ResFilterContactWithUsDto> GetAllContactWithUs(ReqFilterContactWithUsDto filter)
        {

            var contactWithUsQuery = contactWithUsRepo.GetAllEntity();
            var count = (int)Math.Ceiling(contactWithUsQuery.Count() / (double)filter.TakeEntity);
            var pager = Pager.Build(count, filter.PageId, filter.TakeEntity);
            var list = await contactWithUsRepo.GetAllEntity().Paging(pager).Where(u => u.IsRemove == false).Select(x => new GetAllContactWithUsDto { Message=x.Expration,UserFirstName = x.UserFirstName, UserLastName = x.UserLastName, UserEmail = x.UserEmail, UserPhoneNumber = x.UserPhoneNumber }).ToListAsync();    //use the genric interface options and save values in variable
            var result = new ResFilterContactWithUsDto();
            result.SetPaging(pager);
            return result.SetContactWithUses(list);

        }

        #endregion

        #region answer to message
        public async Task<bool> AnswerToMessage(string ToEmail, string subject, string Content,List<IFormFile>? Attachments)
        {
           var res =await mailService.SendEmailAsync(new MailRequestDto(){ToEmail=ToEmail,Body=Content,Subject=subject,Attachments=Attachments});
            return res;
        }
        #endregion
    }
}
