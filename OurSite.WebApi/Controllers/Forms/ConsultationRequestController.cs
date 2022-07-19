using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurSite.Core.DTOs;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Services.Repositories;
using OurSite.Core.Utilities;
using OurSite.DataLayer.Contexts;
using OurSite.DataLayer.Entities.ConsultationRequest;

namespace OurSite.WebApi.Controllers.Forms
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



        #region send form with file
        /// <summary>
        /// send file in forms and tickets {Get request from form}
        /// </summary>
        /// <param name="sendConsultationFormWithFile"></param>
        /// <returns></returns>
        [HttpPost("send-form-with-file")]
        public async Task<IActionResult> SendConsultationForm([FromForm] ConsultationRequestDto sendConsultationFormWithFile)
        {

            if (sendConsultationFormWithFile.SubmittedFile != null)
            {
                string path = Directory.GetCurrentDirectory() + "\\wwwroot\\uploads\\";
                var firstResponse = await FileUploader.UploadFile(path, sendConsultationFormWithFile.SubmittedFile, 10);

                switch (firstResponse.Status)
                {
                    case resFileUploader.Success:
                        sendConsultationFormWithFile.SubmittedFileName = firstResponse.FileName;
                        break;
                    case resFileUploader.Failure:
                        return JsonStatusResponse.Error("ارسال فایل با خطا مواجه شد");
                    case resFileUploader.ToBig:
                        return JsonStatusResponse.Error("حجم فایل انتخابی بیش از حد مجاز می‌باشد");
                    case resFileUploader.NoContent:
                        return JsonStatusResponse.Error("فایلی برای ارسال انتخاب نشده است");
                    case resFileUploader.InvalidExtention:
                        return JsonStatusResponse.Error("فرمت فایل انتخابی نامناسب می‌باشد");
                    default:
                        return JsonStatusResponse.Error("ارسال فایل با خطا مواجه شد");
                }
            }

            var secondResponse = await consultationRequestService.SendConsultationForm(sendConsultationFormWithFile);
            if (secondResponse)
                return JsonStatusResponse.Success("درخواست با موفقیت ارسال گردید");
            return JsonStatusResponse.Error("درخواست شما ارسال نگردید");
        }
        #endregion
    }
}
