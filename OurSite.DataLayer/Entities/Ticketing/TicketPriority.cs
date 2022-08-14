using OurSite.DataLayer.Entities.BaseEntities;

namespace OurSite.DataLayer.Entities.Ticketing
{
    public class TicketPriority:BaseEntity
    {
        public string Name { get; set; }
        public string Title { get; set; }
    }
}
