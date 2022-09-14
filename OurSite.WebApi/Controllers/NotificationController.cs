using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Utilities;
using OurSite.Core.DTOs.NotificationDtos;
using OurSite.Core.DTOs.TicketDtos;
using Microsoft.AspNetCore.Authorization;

namespace OurSite.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        #region constructor

        private INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        #endregion

        /// <summary>
        /// create a Notification
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("create-Notification")]
        [Authorize(Policy =StaticPermissions.PermissionCreateNotification)]
        public async Task<IActionResult> CreateNotification([FromBody]ReqCreateNotificationDto request)
        {
            var res = await _notificationService.CreateNotification(request);
            if (res)
            {
                HttpContext.Response.StatusCode = 201;
                return JsonStatusResponse.Success("Notification has been created successfully");
            }
            HttpContext.Response.StatusCode = 500;
            return JsonStatusResponse.Error("Notification was not created");
        }


        /// <summary>
        /// delete Notifications
        /// </summary>
        /// <param name="notificationsId"></param>
        /// <returns></returns>
        [HttpDelete("Delete-Notifications")]
        public async Task<IActionResult> DeleteNotification([FromBody]List<long> notificationsId)
        {
            var res = await _notificationService.DeleteNotifications(notificationsId);
            if (res)
            {
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success("Notifications have been deleted successfully");
            }
            HttpContext.Response.StatusCode = 500;
            return JsonStatusResponse.Error("Notifications were not deleted");
        }



        /// <summary>
        /// get a Notification details
        /// </summary>
        /// <param name="NotificationId"></param>
        /// <returns></returns>
        [HttpGet("Get-Notification")]
        public async Task<IActionResult> GetNotification(long NotificationId)
        {
            var res = await _notificationService.GetNotification(NotificationId);
            if (res is not null)
            {
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success(res, "successfull");
            }
            HttpContext.Response.StatusCode = 404;
            return JsonStatusResponse.Error("Notification not found");
        }
        /// <summary>
        /// update Notification , Enter the fields you want to update
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("Update-Notification")]
        [Authorize(Policy = StaticPermissions.PermissionUpdateNotification)]

        public async Task<IActionResult> UpdateNotification([FromBody]ReqUpdateNotificationDto request)
        {
            var res = await _notificationService.UpdateNotification(request);
            switch (res)
            {
                case ResOperation.Success:
                    HttpContext.Response.StatusCode = 200;
                    return JsonStatusResponse.Success("Notifications have been Updated successfully");
                case ResOperation.Failure:
                    HttpContext.Response.StatusCode = 500;
                    return JsonStatusResponse.Error("server error");
                case ResOperation.NotFound:
                    HttpContext.Response.StatusCode = 404;
                    return JsonStatusResponse.Error("Notifications not found");
                case ResOperation.UserNotFound:
                    HttpContext.Response.StatusCode = 404;
                    return JsonStatusResponse.Error("Account not found");
                default:
                    HttpContext.Response.StatusCode = 500;
                    return JsonStatusResponse.UnhandledError();
            }
        }
        /// <summary>
        /// get list of Notification
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all-Notifications")]
        public async Task<IActionResult> GetAllNotifications(Guid accountUUId)
        {
            var res = await _notificationService.GetAllNotificationOfAccount(accountUUId);
            if (res is not null && res.Count > 0)
            {
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success(res, "successfull");
            }
            HttpContext.Response.StatusCode = 404;
            return JsonStatusResponse.Error("no notification found");
        }
        /// <summary>
        /// Mark notifications as read, send list of notification Ids in body
        /// </summary>
        /// <param name="notificationsId"></param>
        /// <returns></returns>
        [HttpPut("notification-markAsRead")]
        
        public async Task<IActionResult> MarkAsRead([FromBody]List<long> notificationsId)
        {
            var res = await _notificationService.MarkAsRead(notificationsId);
            if (res)
            {
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success("Success");
            }
            HttpContext.Response.StatusCode = 500;
            return JsonStatusResponse.Error("Error");
        }
    }
}
