using System;
using Microsoft.AspNetCore.Http;
using OurSite.DataLayer.Entities.Projects;

namespace OurSite.Core.DTOs.ProjectDtos
{
    public class ReqUploadContractDto
    {
        public long ProjectId { get; set; }
        public IFormFile ContractFile { get; set; }
    }

    public enum resUploadContract{
        projectNotFound,
        Success,
        FileNotFound,
        Error,
        TooBig,
        FileExtentionError
    }
}

