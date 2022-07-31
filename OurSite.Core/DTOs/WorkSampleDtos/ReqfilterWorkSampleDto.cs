namespace OurSite.Core.DTOs.WorkSampleDtos;
public class ReqFilterWorkSampleDto
{
    public int PageId { get; set; }
    public int TakeEntity { get; set; }
    public WorkSampleOrderBy? OrderBy { get; set; }
    public List<long>? CategoriesId { get; set; }
}
public enum WorkSampleOrderBy
{
    DateAsc,
    DateDec,
    LikeAsc,
    LikeDec
}