using System;
namespace OurSite.Core.DTOs.ProjectDtos
{
    public class DeleteProjectDto
    {
        public long ProId { get; set; }
        public long? AdminId { get; set; }
        public long? UserId { get; set; }

    }
}

