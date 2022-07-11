using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Utilities
{
    public static class PathTools
    {
        public const string Domain = "https://localhost:7181";

        public const string ConsultationFile = Domain + "/upload/";
        public const string ProfilePhotos = Domain + "/upload/Profiles";

        public static string FileUploadPath = Directory.GetCurrentDirectory() + "//Content//File";
    }
}
