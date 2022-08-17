using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.NotificationDtos
{
    public class ReqCreateNotificationDto
    {
        public Guid AccountUUID { get; set; }
        public string Message { get; set; }
        public string? Url { get; set; }
    }
}
