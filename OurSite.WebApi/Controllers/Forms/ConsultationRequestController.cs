using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurSite.Core.DTOs.ConsultationRequestDtos;
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


        #region Send ConsultationRequest Form

        #region send form with file
        /// <summary>
        /// Registers a consultation form.The form can contain file and checkboxes
        /// Note: Do not submit "SubmittedFileName" This is an unimportant field
        /// Returns:
        /// Success("درخواست با موفقیت ارسال گردید")
        /// Error("ارسال فایل با خطا مواجه شد")
        /// Error("فایلی برای ارسال انتخاب نشده است")
        /// Error("حجم فایل انتخابی بیش از حد مجاز می‌باشد")
        /// Error("فرمت فایل انتخابی نامناسب می‌باشد")
        /// Error("فرمت فایل انتخابی نامناسب می‌باشد")
        /// </summary>
        /// <remarks>The file size must be less than 10 MB</remarks>
        /// <param name="sendConsultationFormWithFile"></param>
        /// <returns></returns>
        [HttpPost("send-form-with-file")]
        public async Task<IActionResult> SendConsultationForm([FromForm] ConsultationRequestDto sendConsultationFormWithFile)
        {

            if (sendConsultationFormWithFile.SubmittedFile != null)
            {
                // string path = Directory.GetCurrentDirectory() + "\\wwwroot\\uploads\\";
                var firstResponse = await FileUploader.UploadFile(PathTools.ConsultationFilePath, sendConsultationFormWithFile.SubmittedFile, 10);

                switch (firstResponse.Status)
                {
                    case resFileUploader.Success:
                        {
                            var secondResponse = await consultationRequestService.SendConsultationForm(sendConsultationFormWithFile,firstResponse.FileName);
                            if (secondResponse)
                            {
                                HttpContext.Response.StatusCode = 200;
                                return JsonStatusResponse.Success("Request has been success successfully");
                            }
                            HttpContext.Response.StatusCode = 400;
                            return JsonStatusResponse.Error("send request failed.");
                        }
                    case resFileUploader.Failure:
                        HttpContext.Response.StatusCode = 500;
                        return JsonStatusResponse.Error("send request faild, try agian later");
                    case resFileUploader.ToBig:
                        HttpContext.Response.StatusCode = 413;
                        return JsonStatusResponse.Error("The file size is large");
                    case resFileUploader.NoContent:
                        HttpContext.Response.StatusCode = 204;
                        return JsonStatusResponse.Error("File didn't choosed");
                    case resFileUploader.InvalidExtention:
                        HttpContext.Response.StatusCode = 400;
                        return JsonStatusResponse.InvalidInput();
                    default:
                        HttpContext.Response.StatusCode = 500;
                        return JsonStatusResponse.UnhandledError();
                }
            }
            var res = await consultationRequestService.SendConsultationForm(sendConsultationFormWithFile,null);
            if (res)
            {
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success("Request send sucessfully");
            }
                
            HttpContext.Response.StatusCode = 500;
            return JsonStatusResponse.UnhandledError();            
            
        }
        #endregion

        #region ConsultationRequest All Form
        /// <summary>
        /// Get all Consultation forms filtered by pagination
        /// Returns: Success with Return Data ,NotFound
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("view-all-ConsultationRequest")]
        public async Task<IActionResult> GetAllConsultationRequest([FromQuery] ReqFilterConsultationRequestDto filter)
        {
            var consultationRequest = await consultationRequestService.GetAllConsultationRequest(filter);
            if (consultationRequest.ConsultationRequests is not null)
            {
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success(message: "success", ReturnData: consultationRequest);
            }
            HttpContext.Response.StatusCode = 404;
            return JsonStatusResponse.NotFound("consulation not found");

        }
        #endregion

        #region Get Consulation Form
        /// <summary>
        /// Getting a consultation form with ID
        /// Returns: Success with data  , NotFound
        /// </summary>
        /// <param name="ConsultationFormId"></param>
        /// <returns></returns>
        [HttpGet("get-consulationFtom/{ConsultationFormId}")]
        public async Task<IActionResult> GetConsulationForm([FromRoute] long ConsultationFormId)
        {
            var res = await consultationRequestService.GetConsulationForm(ConsultationFormId);
            if (res is not null)
            {
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success(res, "success");
            }
            HttpContext.Response.StatusCode = 404;
            return JsonStatusResponse.NotFound("consulation not found");
        }
        #endregion

        #region change status consulations
        /// <summary>
        /// change read status of consultion form , Unread and read
        /// </summary>
        /// <param name="ConsulationId"></param>
        /// <returns></returns>
        [HttpPut("Change-Consulation-Read-Status")]
        public async Task<IActionResult> ChangeReadStatus(long ConsulationId){
            var res= await consultationRequestService.ChangeReadStatus(ConsulationId);
            if (res)
            {
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success("The read status of the form has changed");
            }
            HttpContext.Response.StatusCode = 404;
            return JsonStatusResponse.NotFound("consulation not found");
        }
        #endregion




        #endregion
    }
}
