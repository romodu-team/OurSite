﻿using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.Utilities
{
    public static class PathTools
    {

        public const string Domain = "https:\\localhost:7181";

       // public const string ConsultationFile = Domain + "/upload/";
        public static string ProfilePhotos = Path.Combine(Directory.GetCurrentDirectory(), "upload\\images\\Profiles");
        public static string GetProfilePhotos = "/upload/images/Profiles/";

        public static string FileUploadPath =Path.Combine(Directory.GetCurrentDirectory() ,"upload\\File");
    }
}
