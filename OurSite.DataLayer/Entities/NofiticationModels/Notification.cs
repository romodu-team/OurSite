using OurSite.DataLayer.Entities.BaseEntities;

namespace OurSite.DataLayer.Entities.NotificationModels;


public class Notification:BaseEntity
{
    public string EmployeeName { get; set; }  
    public string TranType { get; set; }  
    public string Message { get; set; }
}
