using Microsoft.AspNetCore.Hosting;
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
        
        public static string ImageGallery = Path.Combine(Directory.GetCurrentDirectory(), "upload\\images\\Gallery");
        public static string GetImageGallery = "/upload/images/Gallery/";
        public static string WorkSampleImages = Path.Combine(Directory.GetCurrentDirectory(), "upload\\images\\WorkSample");
        public static string GetWorkSampleImages = "/upload/images/WorkSample/";
        public static string FileUploadPath =Path.Combine(Directory.GetCurrentDirectory() ,"upload\\File");
        public static string ConsultationFilePath =Path.Combine(Directory.GetCurrentDirectory() , "upload\\File\\Consultation");
        public static string GetConsultationFilePath = "/upload/file/Consultation/";

        public static string ContractUploadPath =Path.Combine(Directory.GetCurrentDirectory() ,"upload\\File\\Contracts");
        public static string ContractUploadName = "/upload/file/contract/";

        public static string TicketAttachmentUploadPath = Path.Combine(Directory.GetCurrentDirectory(), "upload\\File\\TicketAttachment");
        public static string TicketAttachmentDownloadPath = "/upload/file/TicketAttachment/";
    }
}
