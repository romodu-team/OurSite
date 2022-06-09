using Microsoft.AspNetCore.Http;
using OurSite.DataLayer.Entities.CheckBoxItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurSite.Core.DTOs
{
    public class ConsultationRequestDto
    {
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhoneNumber { get; set; }
        public string Expration { get; set; }
        public IFormFile? SubmittedFile { get; set; }
        public string? FileName { get; set; }
    }
}
