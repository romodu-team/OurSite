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
        public static async Task<ResUploadDto> UploadFile(string path, IFormFile file, int maxSizeMb)
        {
            if (file != null)
            {
                try
                {
                    var size = file.Length/1000;
                    if (size > maxSizeMb * 1024)
                        return new ResUploadDto { Status = resFileUploader.ToBig, FileName = null };

                    string fileExtention = Path.GetExtension(file.FileName);

                    string[] acceptedFileTypes = new string[7];
                    acceptedFileTypes[0] = ".pdf";
                    acceptedFileTypes[1] = ".doc";
                    acceptedFileTypes[2] = ".docx";
                    acceptedFileTypes[3] = ".jpg";
                    acceptedFileTypes[4] = ".jpeg";
                    acceptedFileTypes[5] = ".gif";
                    acceptedFileTypes[6] = ".png";

                    bool acceptFile = false;

                    //should we accept the file?
                    for (int i = 0; i <= 6; i++)
                    {
                        if (fileExtention == acceptedFileTypes[i])
                        {
                            //accept the file, yay!
                            acceptFile = true;
                            break;
                        }
                    }

                    if (!acceptFile)
                    {
                        return new ResUploadDto { Status = resFileUploader.InvalidExtention, FileName = null };
                    }

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string FileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    using (FileStream fileStream = System.IO.File.Create(path +"//"+ FileName))
                    {
                        await file.CopyToAsync(fileStream);
                        fileStream.Flush();

                    }
                    return new ResUploadDto { Status = resFileUploader.Success, FileName = FileName };
                }
                catch
                {
                    return new ResUploadDto { Status = resFileUploader.Failure, FileName = null };
                }
            }
            else
            {
                return new ResUploadDto { Status = resFileUploader.NoContent, FileName = null };
            }
        }
    }
    public enum resFileUploader
    {
        Success,
        Failure,
        ToBig,
        NoContent,
        InvalidExtention
    }
}
