using OurSite.Core.Utilities;

namespace OurSite.Core.DTOs.WorkSampleDtos;

public class ResCreateWorkSampleDto
{
    public long? WorkSampleID { get; set; }
    public resFileUploader resCreateWorkSample { get; set; }
}
