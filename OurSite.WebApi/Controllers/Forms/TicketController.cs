using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurSite.Core.DTOs;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Utilities;

namespace OurSite.WebApi.Controllers.Forms
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        #region constructor

        private ITicketService ticketService;
        public TicketController(ITicketService ticketService)
        {
            this.ticketService = ticketService;
        }
        #endregion

        #region Creat ticket
        /// <summary>
        /// creat new ticket by customers {Get request from body}
        /// </summary>
        /// <param name="ticketDto"></param>
        /// <returns></returns>
        [HttpPost("create-ticket")]
        public async Task<IActionResult> CreateTicket([FromBody] TicketDto ticketDto)
        {
            if (ticketDto.SubmittedTicketFile != null)
            {

                var uploadFileResponse = await FileUploader.UploadFile(PathTools.FileUploadPath, ticketDto.SubmittedTicketFile, 10);

                switch (uploadFileResponse.Status)
                {
                    case resFileUploader.Success:
                        ticketDto.SubmittedTicketFileName = uploadFileResponse.FileName;
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

            var res = await ticketService.createTicket(ticketDto);

            switch (res.resTicket)
            {
                case ResTicket.Success:
                    return JsonStatusResponse.Success("تیکت با موفقیت ارسال شد");

                default:
                    return JsonStatusResponse.Error("ارسال تیکت با خطا مواجه شد");
            }
        }
        #endregion


    }
}
