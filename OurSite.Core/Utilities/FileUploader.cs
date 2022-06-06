using Microsoft.AspNetCore.Http;
using OurSite.Core.DTOs.Uploader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Utilities
{
    public static class FileUploader
    {
        public static async Task<ResUploadDto> UploadFile(string path, IFormFile file)
        {
            if (file != null)
            {
                try
                {

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string FileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    using (FileStream fileStream = System.IO.File.Create(path + FileName))
                    {
                        await file.CopyToAsync(fileStream);
                        fileStream.Flush();

                    }
                    return new ResUploadDto { Status = 200, FileName = FileName };
                }
                catch
                {
                    return new ResUploadDto { Status = 400, FileName = null };
                }
            }
            else
            {
                return new ResUploadDto { Status = 401, FileName = null };
            }
        }
    }
}
