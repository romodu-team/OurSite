using OurSite.Core.DTOs.NotificationDtos;
using OurSite.Core.DTOs.TicketDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Services.Interfaces
{
    public interface INotificationService:IDisposable
    {
        Task<bool> CreateNotification(ReqCreateNotificationDto request);
        Task<bool> DeleteNotifications(List<long> NotificationsId);
        Task<List<ResGetNotificationDto>> GetAllNotificationOfAccount(Guid accountUUID);
        Task<bool> MarkAsRead(List<long> NotificationsId);
        Task<ResOperation> UpdateNotification(ReqUpdateNotificationDto request);
        Task<ResGetNotificationDto> GetNotification(long notificationId);

    }
}
