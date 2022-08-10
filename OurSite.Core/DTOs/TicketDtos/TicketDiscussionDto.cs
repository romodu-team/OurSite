namespace OurSite.Core.DTOs.TicketDtos
{
    public class TicketDiscussionDto
    {
        public long SenderId { get; set; }
        public long Id { get; set; }
        public string Content { get; set; }
        public string CreateDate { get; set; }
        public string? AttachmentPath { get; set; }
    }
}