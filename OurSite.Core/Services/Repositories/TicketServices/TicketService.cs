using Microsoft.EntityFrameworkCore;
using OurSite.Core.DTOs.TicketDtos;
using OurSite.Core.Services.Interfaces;
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
        private IGenericReopsitories<TicketModel> _TicketRepository;
        private IGenericReopsitories<TicketDiscussion> _DiscussionRepository;
        private IGenericReopsitories<TicketAttachment> _AttachmentRepository;
        private IGenericReopsitories<TicketStatus> _TicketStatusRepository;
        private IAdminService _AdminService;
        private IUserService _UserService;
        public TicketService(IUserService UserService, IAdminService AdminService, IGenericReopsitories<TicketStatus> TicketStatusRepository, IGenericReopsitories<TicketModel> TicketRepository, IGenericReopsitories<TicketDiscussion> DiscussionRepository, IGenericReopsitories<TicketAttachment> AttachmentRepository)
        {
            _AttachmentRepository = AttachmentRepository;
            _DiscussionRepository = DiscussionRepository;
            _TicketRepository = TicketRepository;
            _TicketStatusRepository = TicketStatusRepository;
            _AdminService = AdminService;
            _UserService = UserService;
        }
        #endregion
        public async Task<ResOperation> ChangeTicketStatus(long TicketId, long StatusId)
        {
            //find ticket
            var ticket = await _TicketRepository.GetEntity(TicketId);
            if(ticket is not null)
            {
                //if status is valid
                var TicketStatus= await _TicketStatusRepository.GetEntity(StatusId);
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
            if (await _TicketRepository.GetAllEntity().AnyAsync(t=>t.Id==request.TicketId))
            {
                //find sender
                bool validSender = false;
                if (request.IsAdmin)
                    validSender = await _AdminService.IsAdminExist(request.SenderId);
                else
                    validSender = await _UserService.IsUserExist(request.SenderId);
                if(validSender)
                {
                    //create discussion
                    var discussion = new TicketDiscussion()
                    {
                        Content=request.Content,
                        SenderId=request.SenderId,
                        TicketId=request.TicketId,
                        IsRemove=false
                        
                    };
                    try
                    {
                        await _DiscussionRepository.AddEntity(discussion);
                        await _DiscussionRepository.SaveEntity();

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
                                    FileSize = (long)res.FileSize,
                                    TicketDiscussionId = discussion.Id
                                };
                                await _AttachmentRepository.AddEntity(attachment);
                                await _AttachmentRepository.SaveEntity();

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
            return new ResCreateDiscussionDto {  AttachmentStatus= resFileUploader.NoContent,DiscussionStatus = ResOperation.NotFound };
        }

        public async Task<ResCreateDiscussionDto> CreateTicket(ReqCreateTicketDto request)
        {
            //if user is exist
            var isUserExist = await _UserService.IsUserExist(request.UserId);
            if(isUserExist)
            {
                //create ticket
                var ticket = new TicketModel
                {
                    UserId = request.UserId,
                    ProjectId = request.ProjectId,
                    SupporterId = request.SupporterId,
                    TicketCategoryId = request.TicketCategoryId,
                    TicketPriorityId = request.TicketPriorityId,
                    TicketStatusId = request.TicketStatusId
                };
                try
                {
                    await _TicketRepository.AddEntity(ticket);
                    await _TicketRepository.SaveEntity();
                    //create first discussion
                    var res = await CreateDiscussion(new ReqCreateDiscussion { Attachment = request.Attachment, Content = request.Description, TicketId = ticket.Id, SenderId = request.SenderId, IsAdmin = request.IsAdmin });
                    return new ResCreateDiscussionDto { AttachmentStatus=res.AttachmentStatus,DiscussionStatus = res.DiscussionStatus };
                }
                catch (Exception)
                {

                    return new ResCreateDiscussionDto { AttachmentStatus = resFileUploader.Failure, DiscussionStatus = ResOperation.Failure };

                }


            }
            return ResOperation.NotFound;

        }

        public Task<ResOperation> DeleteDiscussion()
        {
            throw new NotImplementedException();
        }

        public Task<ResOperation> DeleteTicket()
        {
            throw new NotImplementedException();
        }

        public Task<ResGetAllTicketDto> GetAllTickets()
        {
            throw new NotImplementedException();
        }

        public Task<ResGetTicketDto> GetTicket(long TicketId)
        {
            throw new NotImplementedException();
        }

        public Task<ResGetAllTicketDto> GetTicketAssignedToAdmin(long adminId)
        {
            throw new NotImplementedException();
        }

        public Task<ResGetAllUserTicketsDto> GetUserTickets(long UserId)
        {
            throw new NotImplementedException();
        }

        public Task<TicketReportDto> TicketReport()
        {
            throw new NotImplementedException();
        }

        public Task<ResOperation> UpdateTicket()
        {
            throw new NotImplementedException();
        }
    }

}
