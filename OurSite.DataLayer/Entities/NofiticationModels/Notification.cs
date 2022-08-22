using OurSite.DataLayer.Entities.BaseEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace OurSite.DataLayer.Entities.NotificationModels;


public class Notification:BaseEntity
{
    public Guid AccountUUID { get; set; }  
    public bool IsRead { get; set; }  
    public string Message { get; set; }
    public string? Url { get; set; }
}
