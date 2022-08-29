using Microsoft.EntityFrameworkCore;
using OurSite.Core.DTOs.MailDtos;
using OurSite.Core.DTOs.NotificationDtos;
using OurSite.Core.DTOs.Paging;
using OurSite.Core.DTOs.TicketDtos;
using OurSite.Core.DTOs.UserDtos;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Services.Interfaces.Mail;
using OurSite.Core.Services.Interfaces.TicketInterfaces;
using OurSite.Core.Utilities;
using OurSite.DataLayer.Entities.Ticketing;
using OurSite.DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Services.Repositories.TicketServices
{
    public class TicketService : ITicketService
    {
        #region constructor
        private IGenericRepository<TicketModel> _TicketRepository;
        private IGenericRepository<TicketDiscussion> _DiscussionRepository;
        private IGenericRepository<TicketAttachment> _AttachmentRepository;
        private IGenericRepository<TicketStatus> _TicketStatusRepository;
        private IAdminService _AdminService;
        private IUserService _UserService;
        private INotificationService _notificationService;
        private IMailService _mailService;
        public TicketService(IMailService mailService, INotificationService notificationService, IUserService UserService, IAdminService AdminService, IGenericRepository<TicketStatus> TicketStatusRepository, IGenericRepository<TicketModel> TicketRepository, IGenericRepository<TicketDiscussion> DiscussionRepository, IGenericRepository<TicketAttachment> AttachmentRepository)
        {
            _mailService = mailService;
            _AttachmentRepository = AttachmentRepository;
            _DiscussionRepository = DiscussionRepository;
            _TicketRepository = TicketRepository;
            _TicketStatusRepository = TicketStatusRepository;
            _AdminService = AdminService;
            _UserService = UserService;
            _notificationService = notificationService;
        }


        public void Dispose()
        {
            _AttachmentRepository.Dispose();
            _DiscussionRepository.Dispose();
            _TicketRepository.Dispose();
            _TicketStatusRepository.Dispose();
            _AdminService.Dispose();
            _UserService.Dispose();
        }
        #endregion
        public async Task<ResOperation> ChangeTicketStatus(long TicketId, long StatusId)
        {
            //find ticket
            var ticket = await _TicketRepository.GetEntity(TicketId);
            if (ticket is not null && ticket.IsRemove == false)
            {
                //if status is valid
                var TicketStatus = await _TicketStatusRepository.GetEntity(StatusId);
                if (TicketStatus is not null)
                {
                    //change status
                    ticket.TicketStatusId = TicketStatus.Id;
                    try
                    {
                        _TicketRepository.UpDateEntity(ticket);
                        await _TicketRepository.SaveEntity();
                        return ResOperation.Success;
                    }
                    catch (Exception)
                    {

                        return ResOperation.Failure;
                    }

                }
                return ResOperation.StatusNotFound;


            }
            return ResOperation.NotFound;
        }

        public async Task<ResCreateDiscussionDto> CreateDiscussion(ReqCreateDiscussion request)
        {
            //find ticket
            var ticket = await _TicketRepository.GetAllEntity().Include(t=>t.TicketStatus).SingleOrDefaultAsync(t => t.Id == request.TicketId);
            if (ticket is not null)
            {
                if (ticket.TicketStatus.Name != "Closed")
                {
                    //find sender
                    bool validSender = false;
                    if (request.IsAdmin)
                        validSender = await _AdminService.IsAdminExist(request.SenderId);
                    else
                        validSender = await _UserService.IsUserExist(request.SenderId);
                    if (validSender)
                    {
                        //create discussion
                        var discussion = new TicketDiscussion()
                        {
                            Content = request.Content,
                            SenderId = request.SenderId,
                            TicketId = request.TicketId,
                            IsAdmin = request.IsAdmin,
                            IsRemove = false

                        };
                        try
                        {
                            await _DiscussionRepository.AddEntity(discussion);
                            await _DiscussionRepository.SaveEntity();
                            //send notification and email
                            //find accaount
                            if (!request.IsAdmin)//if sender is user , send notificatin to admin
                            {
                                if (ticket.SupporterId != null)
                                {
                                    var account = await _AdminService.GetAdminById((long)ticket.SupporterId);
                                    await _notificationService.CreateNotification(new ReqCreateNotificationDto { AccountUUID = account.UUID, Message = $"کاربر پاسخ جدیدی برای تیکت شماره {ticket.Id} ارسال کرد" });
                                    await _mailService.SendEmailAsync(new MailRequestDto { ToEmail = account.Email, Subject = $"کاربر به تیکت شماره {ticket.Id} پاسخ داد.", Attachments = null, Body=$"{ticket.Title}\n{discussion.Content}\n{ticket.Id}\n{ticket.TicketStatus.Title}"});
                                }
                                
                            }
                            else // sender is admin ,send notification to User
                            {
                                var account = await _UserService.GetUserById((long)ticket.UserId);
                                await _notificationService.CreateNotification(new ReqCreateNotificationDto { AccountUUID = account.UUID, Message = $"پشتیبان پاسخ جدیدی برای تیکت شماره {ticket.Id} ارسال کرد" } );
                                if(account.Email!=null)
                                    await _mailService.SendEmailAsync(new MailRequestDto { ToEmail = account.Email, Subject = $"پشتیبان به تیکت شماره {ticket.Id} پاسخ داد.", Attachments = null, Body = $"{ticket.Title}\n{discussion.Content}\n{ticket.Id}\n{ticket.TicketStatus.Title}" });
                            }
                            //upload attachment
                            if (request.Attachment is not null)
                            {
                                var res = await FileUploader.UploadTicketAttachment(PathTools.TicketAttachmentUploadPath, request.Attachment, 20);
                                if (res.Status == resFileUploader.Success)
                                {
                                    var attachment = new TicketAttachment()
                                    {
                                        ContentType = res.ContentType,
                                        FileName = res.FileName,
                                        FileSize = (float)res.FileSize,
                                        TicketDiscussionId = discussion.Id
                                    };
                                    await _AttachmentRepository.AddEntity(attachment);
                                    await _AttachmentRepository.SaveEntity();
                                    discussion.AttachmentId = attachment.Id;
                                    _DiscussionRepository.UpDateEntity(discussion);
                                    await _DiscussionRepository.SaveEntity();
                                    return new ResCreateDiscussionDto { AttachmentStatus = resFileUploader.Success, DiscussionStatus = ResOperation.Success };

                                }
                                else
                                    return new ResCreateDiscussionDto { AttachmentStatus = res.Status, DiscussionStatus = ResOperation.Success };
                            }

                            return new ResCreateDiscussionDto { AttachmentStatus = resFileUploader.NoContent, DiscussionStatus = ResOperation.Success };
                        }
                        catch (Exception)
                        {
                            return new ResCreateDiscussionDto { DiscussionStatus = ResOperation.Failure, AttachmentStatus = resFileUploader.Failure };

                        }

                    }
                    return new ResCreateDiscussionDto { DiscussionStatus = ResOperation.SenderNotFound, AttachmentStatus = resFileUploader.NoContent };
                }
                return new ResCreateDiscussionDto { DiscussionStatus = ResOperation.notAllowed, AttachmentStatus = resFileUploader.NoContent };

            }
            return new ResCreateDiscussionDto { AttachmentStatus = resFileUploader.NoContent, DiscussionStatus = ResOperation.NotFound };
        }

        public async Task<ResCreateDiscussionDto> CreateTicket(ReqCreateTicketDto request)
        {
            //if user is exist
            var isUserExist = await _UserService.IsUserExist(request.UserId);
            if (isUserExist)
            {
                //create ticket
                var ticket = new TicketModel
                {
                    Title=request.Title,
                    UserId = request.UserId,
                    ProjectId = request.ProjectId,
                    SupporterId = request.SupporterId,
                    TicketCategoryId = request.TicketCategoryId,
                    TicketPriorityId = request.TicketPriorityId,
                    TicketStatusId = request.TicketStatusId,
                    
                };
                try
                {
                    await _TicketRepository.AddEntity(ticket);
                    await _TicketRepository.SaveEntity();
                    //send notification and email
                    //find accaount
                    if (!request.IsAdmin)//if sender is user , send notificatin to  admins and send mail to site email
                    {

                        var accounts = await _AdminService.GetAllAdmin(new ReqFilterUserDto { PageId=1,TakeEntity=1000});
                        if(accounts.Admins!=null && accounts.Admins.Count > 0)
                        {
                            foreach (var admin in accounts.Admins)
                            {
                                await _notificationService.CreateNotification(new ReqCreateNotificationDto { AccountUUID = admin.UUID, Message = $"تیکت جدید با نام {ticket.Title} دریافت شد." });
                            }
                        }

                        await _mailService.SendEmailAsync(new MailRequestDto { ToEmail = "Support@romodu.com", Subject = $"تیکت جدید با نام {ticket.Title} دریافت شد.", Attachments = null, Body = $"عنوان تیکت:{ticket.Title}\nمتن تیکت:{request.Description}\nشماره تیکت:{ticket.Id}" });

                    }
                    else // sender is admin ,send notification to User
                    {
                        var account = await _UserService.GetUserById((long)ticket.UserId);
                        await _notificationService.CreateNotification(new ReqCreateNotificationDto { AccountUUID = account.UUID, Message = $"تیکت جدید برای شما با موضوع {ticket.Title} ایجاد شد." });
                        if (account.Email != null)
                            await _mailService.SendEmailAsync(new MailRequestDto { ToEmail = account.Email, Subject = $"تیکت جدید برای شما با موضوع {ticket.Title} ایجاد شد.", Attachments = null, Body = $"عنوان تیکت:{ticket.Title}\nمتن تیکت:{request.Description}\nشماره تیکت:{ticket.Id}" });
                    }
                    //create first discussion
                    var res = await CreateDiscussion(new ReqCreateDiscussion { Attachment = request.Attachment, Content = request.Description, TicketId = ticket.Id, SenderId = request.SenderId, IsAdmin = request.IsAdmin });
                    return new ResCreateDiscussionDto { AttachmentStatus = res.AttachmentStatus, DiscussionStatus = res.DiscussionStatus };
                }
                catch (Exception)
                {

                    return new ResCreateDiscussionDto { AttachmentStatus = resFileUploader.Failure, DiscussionStatus = ResOperation.Failure };

                }


            }
            return new ResCreateDiscussionDto { AttachmentStatus = resFileUploader.NoContent, DiscussionStatus = ResOperation.UserNotFound };

        }

        public async Task<ResOperation> DeleteDiscussion(List<long> DiscussionsId)
        {
            try
            {

                foreach (var item in DiscussionsId)
                {
                    //delete discussion
                    await _DiscussionRepository.DeleteEntity(item);

                    //delete attachment
                    var attachment = await _AttachmentRepository.GetAllEntity().SingleOrDefaultAsync(a => a.TicketDiscussionId == item && a.IsRemove == false);
                    if (attachment != null)
                    {
                        FileUploader.DeleteFile(PathTools.TicketAttachmentUploadPath + "\\" + attachment.FileName);
                        await _AttachmentRepository.DeleteEntity(attachment.Id);

                    }
                }

                await _DiscussionRepository.SaveEntity();
                await _AttachmentRepository.SaveEntity();



                return ResOperation.Success;
            }
            catch (Exception)
            {

                return ResOperation.Failure;
            }

        }

        public async Task<ResOperation> DeleteTicket(long TicketId)
        {

            try
            {//delete ticket
                var res = await _TicketRepository.DeleteEntity(TicketId);
                await _TicketRepository.SaveEntity();
                if (res)
                {  //delete related discussion
                    var discussionsId = await _DiscussionRepository.GetAllEntity().Where(d => d.TicketId == TicketId && d.IsRemove == false).Select(d => d.Id).ToListAsync();
                    await DeleteDiscussion(discussionsId);
                    return ResOperation.Success;
                }
                return ResOperation.Failure;
            }
            catch (Exception)
            {

                return ResOperation.Failure;
            }

        }


        public async Task<ResFilteredGetAllTicketDto> GetAllTickets(ReqGetAllTicketDto request)
        {
            var TicketQuery = _TicketRepository.GetAllEntity().Where(w => w.IsRemove == false)
                .Include(t => t.Supporter).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketCategory).Include(t => t.User)
                .AsQueryable();

            //orderby
            if (request.OrderByDate is not null)
            {
                switch (request.OrderByDate)
                {
                    case OrderByDate.CreateDateAsc:
                        TicketQuery = TicketQuery.OrderBy(t => t.CreateDate);
                        break;
                    case OrderByDate.CreateDateDesc:
                        TicketQuery = TicketQuery.OrderByDescending(t => t.CreateDate);
                        break;
                    case OrderByDate.LastUpdateDateAsc://check if likes is null for errors
                        TicketQuery = TicketQuery.OrderBy(t => t.LastUpdate);
                        break;
                    case OrderByDate.LastUpdateDateDesc:
                        TicketQuery = TicketQuery.OrderByDescending(t => t.LastUpdate);
                        break;
                    default:
                        break;
                }
            }
            //search by ticket name
            if (request.TicketName != null)
                TicketQuery = TicketQuery.Where(t => t.Title.Contains(request.TicketName));
            //search by User email or user name
            if (request.UserNameOrUserEmail != null)
            {
                TicketQuery = TicketQuery.Where(t => t.User.Email.Contains(request.UserNameOrUserEmail) || t.User.UserName.Contains(request.UserNameOrUserEmail));
            }

            //filter by priority
            if (request.PriorityId != null)
                TicketQuery = TicketQuery.Where(t => t.TicketPriorityId == request.PriorityId);
            //filter by status
            if (request.StatusId != null)
                TicketQuery = TicketQuery.Where(t => t.TicketStatusId == request.StatusId);
            //filter by category
            if (request.CategoryId != null)
                TicketQuery = TicketQuery.Where(t => t.TicketCategoryId == request.CategoryId);
            //pagination

            var count = (int)Math.Ceiling(TicketQuery.Count() / (double)request.TakeEntity);
            var pager = Pager.Build(count, request.PageId, request.TakeEntity);

            var list = await TicketQuery.Select(t => new GetAllTicketDto { AssignedTo = String.Concat(t.Supporter.FirstName," ", t.Supporter.LastName), Category = t.TicketCategory.Title, CreateDate = t.CreateDate.ToShortDateString(), LastUpdateDate = t.LastUpdate.ToShortDateString(), Name = t.Title, UserName = t.User.UserName, Priority = t.TicketPriority.Title, Status = t.TicketStatus.Title, Id = t.Id }).ToListAsync();

            var result = new ResFilteredGetAllTicketDto();
            result.SetPaging(pager);
            return result.SetTickets(list);
        }

        public async Task<ResGetTicketDto> GetTicket(long TicketId)
        {
            //get ticket with related entities
            var ticket = await _TicketRepository.GetAllEntity().Include(t => t.Supporter).Include(t => t.User)
                .Include(t => t.TicketStatus).Include(t => t.TicketPriority).Include(t => t.TicketCategory).SingleOrDefaultAsync(t => t.Id == TicketId);
            if (ticket != null)
            {
                //get discussions with attachments
                var discussions = await _DiscussionRepository.GetAllEntity().Include(d => d.Attachment).Where(d => d.TicketId == TicketId && d.IsRemove == false).ToListAsync();
                var resultGetTicket = new ResGetTicketDto()
                {
                    Category = ticket.TicketCategory.Title,
                    CreateDate = ticket.CreateDate.ToShortDateString(),
                    LastUpdateDate = ticket.LastUpdate.ToShortDateString(),
                    AssignedTo =ticket.Supporter != null? String.Concat(ticket.Supporter.FirstName," ", ticket.Supporter.LastName):"",
                    Name = ticket.Title,
                    UserEmail = ticket.User.Email,
                    UserFullname = String.Concat(ticket.User.FirstName, ticket.User.LastName),
                    Priority = ticket.TicketPriority.Title,
                    Status = ticket.TicketStatus.Title,
                    ProjectId = ticket.ProjectId,
                    Attachments = new List<TicketAttachmentDto>()
                };
                foreach (var discussion in discussions)
                {
                    if (discussion.Attachment != null)
                    {
                        var attachment = new TicketAttachmentDto() { AttachmentId=discussion.Attachment.Id,ContentType=discussion.Attachment.ContentType,FileName=discussion.Attachment.FileName,FileSize=discussion.Attachment.FileSize,TicketDiscussionId=discussion.Attachment.TicketDiscussionId};
                        resultGetTicket.Attachments.Add(attachment);
                    }
                      
                      
                }
                if(discussions is not null && discussions.Count>0)
                    resultGetTicket.Discussions = discussions.Select(d => new TicketDiscussionDto { Content = d.Content, CreateDate = d.CreateDate.ToShortDateString(), Id = d.Id,IsAdmin=d.IsAdmin ,SenderId = d.SenderId, AttachmentPath =d.Attachment != null? PathTools.TicketAttachmentDownloadPath + d.Attachment.FileName:null }).ToList();
                return resultGetTicket;
            }
            return null;

            ////get supporter
            //if (ticket.SupporterId is not null)
            //{
            //   var supporter  = await _AdminService.GetAdminById((long)ticket.SupporterId);
            //   resultGetTicket.AssignedTo = String.Concat(supporter.FirstName,supporter.LastName);
            //}
            ////get user
            //var user = await _UserService.ViewUser(ticket.UserId);
            //resultGetTicket.UserEmail = user.Email is not null? user.Email:"";
            //resultGetTicket.UserFullname= String.Concat(user.FirstName, user.LastName);
            ////get category
            //var category = await _TicketCategoryService.GetCategory(ticket.TicketCategoryId);
            //resultGetTicket.Category = category.Title;

            ////get status

            ////get priority

        }

        public async Task<ResFilteredGetAllTicketDto> GetTicketAssignedToAdmin(long adminId, ReqGetAllTicketDto request)
        {
            var TicketQuery = _TicketRepository.GetAllEntity().Where(w => w.SupporterId == adminId && w.IsRemove == false)
                .Include(t => t.Supporter).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketCategory).Include(t => t.User)
                .AsQueryable();

            //orderby
            if (request.OrderByDate is not null)
            {
                switch (request.OrderByDate)
                {
                    case OrderByDate.CreateDateAsc:
                        TicketQuery = TicketQuery.OrderBy(t => t.CreateDate);
                        break;
                    case OrderByDate.CreateDateDesc:
                        TicketQuery = TicketQuery.OrderByDescending(t => t.CreateDate);
                        break;
                    case OrderByDate.LastUpdateDateAsc://check if likes is null for errors
                        TicketQuery = TicketQuery.OrderBy(t => t.LastUpdate);
                        break;
                    case OrderByDate.LastUpdateDateDesc:
                        TicketQuery = TicketQuery.OrderByDescending(t => t.LastUpdate);
                        break;
                    default:
                        break;
                }
            }
            //search by ticket name
            if (request.TicketName != null)
                TicketQuery = TicketQuery.Where(t => t.Title.Contains(request.TicketName));
            //search by User email or user name
            if (request.UserNameOrUserEmail != null)
            {
                TicketQuery = TicketQuery.Where(t => t.User.Email.Contains(request.UserNameOrUserEmail) || t.User.UserName.Contains(request.UserNameOrUserEmail));
            }

            //filter by priority
            if (request.PriorityId != null)
                TicketQuery = TicketQuery.Where(t => t.TicketPriorityId == request.PriorityId);
            //filter by status
            if (request.StatusId != null)
                TicketQuery = TicketQuery.Where(t => t.TicketStatusId == request.StatusId);
            //filter by category
            if (request.CategoryId != null)
                TicketQuery = TicketQuery.Where(t => t.TicketCategoryId == request.CategoryId);
            //pagination

            var count = (int)Math.Ceiling(TicketQuery.Count() / (double)request.TakeEntity);
            var pager = Pager.Build(count, request.PageId, request.TakeEntity);

            var list = await TicketQuery.Select(t => new GetAllTicketDto { AssignedTo = String.Concat(t.Supporter.FirstName," ", t.Supporter.LastName), Category = t.TicketCategory.Title, CreateDate = t.CreateDate.ToShortDateString(), LastUpdateDate = t.LastUpdate.ToShortDateString(), Name = t.Title, UserName = t.User.UserName, Priority = t.TicketPriority.Title, Status = t.TicketStatus.Title, Id = t.Id }).ToListAsync();

            var result = new ResFilteredGetAllTicketDto();
            result.SetPaging(pager);
            return result.SetTickets(list);
        }

        public async Task<ResFilteredGetAllUserTicketDto> GetUserTickets(long UserId, ReqGetAllUserTicketsDto request)
        {
            var TicketQuery = _TicketRepository.GetAllEntity().Where(w => w.UserId == UserId && w.IsRemove == false)
                .Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketCategory).Include(t => t.User)
                .AsQueryable();

            //orderby
            if (request.OrderByDate is not null)
            {
                switch (request.OrderByDate)
                {
                    case OrderByDate.CreateDateAsc:
                        TicketQuery = TicketQuery.OrderBy(t => t.CreateDate);
                        break;
                    case OrderByDate.CreateDateDesc:
                        TicketQuery = TicketQuery.OrderByDescending(t => t.CreateDate);
                        break;
                    case OrderByDate.LastUpdateDateAsc://check if likes is null for errors
                        TicketQuery = TicketQuery.OrderBy(t => t.LastUpdate);
                        break;
                    case OrderByDate.LastUpdateDateDesc:
                        TicketQuery = TicketQuery.OrderByDescending(t => t.LastUpdate);
                        break;
                    default:
                        break;
                }
            }
            //search by ticket name
            if (request.TicketName != null)
                TicketQuery = TicketQuery.Where(t => t.Title.Contains(request.TicketName));

            //filter by status
            if (request.StatusId != null)
                TicketQuery = TicketQuery.Where(t => t.TicketStatusId == request.StatusId);

            //pagination

            var count = (int)Math.Ceiling(TicketQuery.Count() / (double)request.TakeEntity);
            var pager = Pager.Build(count, request.PageId, request.TakeEntity);

            var list = await TicketQuery.Select(t => new GetAllUserTicketsDto { Category = t.TicketCategory.Title, CreateDate = t.CreateDate.ToShortDateString(), LastUpdateDate = t.LastUpdate.ToShortDateString(), Name = t.Title, Priority = t.TicketPriority.Title, Status = t.TicketStatus.Title, Id = t.Id }).ToListAsync();

            var result = new ResFilteredGetAllUserTicketDto();
            result.SetPaging(pager);
            return result.SetTickets(list);
        }
        //باید تست شود حتما
        public async Task<TicketReportDto> TicketReport()
        {
            var report = new TicketReportDto();
            report.TotalTicket = await _TicketRepository.GetAllEntity().Where(t => t.IsRemove == false).CountAsync();
            report.TotalSolvedTicket = await _TicketRepository.GetAllEntity().Include(t => t.TicketStatus).Where(t => t.TicketStatus.Name == "Closed" && t.IsRemove == false).CountAsync();
            var resGroupByUserId = _TicketRepository.GetAllEntity().Where(u=>u.IsRemove==false).AsEnumerable().GroupBy(u=>u.UserId);
            report.TotalUser = resGroupByUserId.Count();
            var resGroupByStatusId = _TicketRepository.GetAllEntity().Where(t => t.IsRemove == false).Include(u=>u.TicketStatus).AsEnumerable().GroupBy(t =>t.TicketStatus.Title);

            report.NumberOfTicketsPerStatus = new Dictionary<string, string>();
            foreach (var item in resGroupByStatusId)
            {
                report.NumberOfTicketsPerStatus.Add(item.Key.ToString(), item.Count().ToString());
            }

            return report;
        }

        public async Task<ResOperation> UpdateTicket(ReqUpdateTicketDto request)
        {
            var ticket = await _TicketRepository.GetEntity(request.TicketId);
            if (ticket is not null)
            {
                if (request.SupporterId != null)
                    ticket.SupporterId = request.SupporterId;
                if (request.TicketStatusId is not null)
                    ticket.TicketStatusId = (long)request.TicketStatusId;
                if (request.TicketCategoryId != null)
                    ticket.TicketCategoryId = (long)request.TicketCategoryId;
                if (request.TicketPriorityId != null)
                    ticket.TicketPriorityId = request.TicketPriorityId;
                if(!string.IsNullOrWhiteSpace(request.Title))
                    ticket.Title = request.Title;
                try
                {
                     _TicketRepository.UpDateEntity(ticket);
                    await _TicketRepository.SaveEntity();
                    return ResOperation.Success;
                }
                catch (Exception)
                {

                    return ResOperation.Failure;
                }
            }
            return ResOperation.NotFound;
        }
    }

}
