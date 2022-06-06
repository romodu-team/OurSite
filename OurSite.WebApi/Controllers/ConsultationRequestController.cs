using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurSite.Core.DTOs;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Services.Repositories;
using OurSite.Core.Utilities;
using OurSite.DataLayer.Contexts;
using OurSite.DataLayer.Entities.CheckBoxItem;
using OurSite.DataLayer.Entities.ConsultationRequest;

namespace OurSite.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultationRequestController : ControllerBase
    {
        private IConsultationRequestService consultationRequestService;
        public IWebHostEnvironment webHostEnvironment;

        public ConsultationRequestController(IConsultationRequestService consultationRequestService)
        {
            this.consultationRequestService = consultationRequestService;
        }



        [HttpPost("send-form-with-file")]
        public async Task<IActionResult> PostFile([FromForm] ConsultationRequestDto sendConsultationFormWithFile)
        {

            if (sendConsultationFormWithFile.SubmittedFile != null)
            {
                string path = Directory.GetCurrentDirectory() + "\\wwwroot\\uploads\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string FileName = Guid.NewGuid().ToString() + Path.GetExtension(sendConsultationFormWithFile.SubmittedFile.FileName);
                using (FileStream fileStream = System.IO.File.Create(path + FileName))
                {
                    sendConsultationFormWithFile.SubmittedFile.CopyTo(fileStream);
                    fileStream.Flush();

                }
                sendConsultationFormWithFile.FileName = FileName;
            }
            
           var res= await consultationRequestService.SendConsultationForm(sendConsultationFormWithFile);
            if(res)
                return JsonStatusResponse.Success("درخواست با موفقیت ارسال گردید");
            return JsonStatusResponse.Error("درخواست شما ارسال نگردید");
        }
    }
}
