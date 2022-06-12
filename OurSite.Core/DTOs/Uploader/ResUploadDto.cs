using OurSite.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs.Uploader
{
    public class ResUploadDto
    {
        public resFileUploader Status { get; set; }
        public string? FileName { get; set; }
    }
}
