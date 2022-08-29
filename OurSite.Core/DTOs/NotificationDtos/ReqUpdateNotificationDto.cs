using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.NotificationDtos
{
    public class ReqUpdateNotificationDto
    {
        public long notifId { get; set; }
        public Guid? AccountUUID { get; set; }
        public bool? IsRead { get; set; }
        public string? Message { get; set; }
        public string? Url { get; set; }
    }
}
