using System.ComponentModel.DataAnnotations.Schema;
using OurSite.DataLayer.Entities.BaseEntities;
using OurSite.DataLayer.Entities.WorkSamples;

namespace OurSite.DataLayer.Entities.Comments;

public class Comment:BaseEntity
{
    public string Text { get; set; }
    public string FullName { get; set; }
    public string Title { get; set; }
    public string EmailAddress { get; set; }
    public long ParentCommentId { get; set; }
    [ForeignKey("ParentCommentId")]
    public Comment ParentComment { get; set; }
}
