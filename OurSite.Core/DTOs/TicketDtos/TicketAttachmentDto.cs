namespace OurSite.Core.DTOs.TicketDtos
{
    public class TicketAttachmentDto
    {
        public long AttachmentId { get; set; }
        public long TicketDiscussionId { get; set; }
        public string FileName { get; set; }
        public float FileSize { get; set; }
        public string ContentType { get; set; }
    }
}