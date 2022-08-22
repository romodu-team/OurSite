using Microsoft.EntityFrameworkCore;
using OurSite.Core.DTOs.NotificationDtos;
using OurSite.Core.DTOs.TicketDtos;
using OurSite.Core.Services.Interfaces;
using OurSite.DataLayer.Entities.NotificationModels;
using OurSite.DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Services.Repositories
{
    public class NotificationService : INotificationService
    {
        #region Constructor
        private readonly IGenericRepository<Notification> _notificationRepository;
        private readonly IAdminService _adminService;
        private readonly IUserService _userService;
        public NotificationService(IGenericRepository<Notification> notificationRepository, IUserService userService, IAdminService adminService)
        {
            _notificationRepository = notificationRepository;
            _userService = userService;
            _adminService = adminService;
        }

        #endregion
        public async Task<bool> CreateNotification(ReqCreateNotificationDto request)
        {
            bool accountExist = false;
            accountExist = await _adminService.IsAdminExistByUUId(request.AccountUUID);
            if (accountExist == false)
                accountExist = await _userService.IsUserExistByUUId(request.AccountUUID);

            if (accountExist == false)
                return false;
            else
            {
                Notification notification = new Notification()
                {
                    AccountUUID = request.AccountUUID,
                    Message = request.Message,
                    Url = request.Url,
                    IsRead = false
                };
                try
                {
                    await _notificationRepository.AddEntity(notification);
                    await _notificationRepository.SaveEntity();
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }
            }

        }

        public async Task<bool> DeleteNotifications(List<long> NotificationsId)
        {
            foreach (var notif in NotificationsId)
            {
                await _notificationRepository.DeleteEntity(notif);
            }
            try
            {
                await _notificationRepository.SaveEntity();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public void Dispose()
        {
            _notificationRepository.Dispose();
        }

        public async Task<List<ResGetNotificationDto>> GetAllNotificationOfAccount(Guid accountUUID)
        {
            var notifications = await _notificationRepository.GetAllEntity().Where(n => n.AccountUUID == accountUUID && n.IsRemove == false)
                .Select(n => new ResGetNotificationDto { AccountUUID = n.AccountUUID, IsRead = n.IsRead, Message = n.Message, Url = n.Url, Id = n.Id, CreateDate = n.CreateDate.ToString() }).ToListAsync();
            return notifications;
        }

        public async Task<ResGetNotificationDto> GetNotification(long notificationId)
        {
            var notification = await _notificationRepository.GetEntity(notificationId);
            if (notification != null)
                return new ResGetNotificationDto { AccountUUID = notification.AccountUUID, CreateDate = notification.CreateDate.ToString(), Id = notification.Id, IsRead = notification.IsRead, Message = notification.Message, Url = notification.Url };
            return null;
        }

        public async Task<bool> MarkAsRead(List<long> NotificationsId)
        {
            foreach (var item in NotificationsId)
            {
                var notification = await _notificationRepository.GetEntity(item);
                if (notification != null)
                {
                    notification.IsRead = true;
                    _notificationRepository.UpDateEntity(notification);
                }
            }
            try
            {
                await _notificationRepository.SaveEntity();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<ResOperation> UpdateNotification(ReqUpdateNotificationDto request)
        {
            var notification = await _notificationRepository.GetEntity(request.notifId);
            if (notification is not null)
            {
                if (request.AccountUUID != null)
                {

                    bool accountExist = false;
                    accountExist = await _adminService.IsAdminExistByUUId(request.AccountUUID.Value);
                    if (accountExist == false)
                        accountExist = await _userService.IsUserExistByUUId(request.AccountUUID.Value);

                    if (accountExist == false)
                        return ResOperation.UserNotFound;
                    else
                        notification.AccountUUID = (Guid)request.AccountUUID;
                }

                if (request.IsRead != null)
                    notification.IsRead = (bool)request.IsRead;
                if (request.Message != null)
                    notification.Message = request.Message;
                if (request.Url != null)
                    notification.Url = request.Url;

                try
                {
                    _notificationRepository.UpDateEntity(notification);
                    await _notificationRepository.SaveEntity();
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
