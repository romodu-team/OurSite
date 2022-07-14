using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurSite.Core.DTOs.TicketsDtos;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Utilities;

namespace OurSite.WebApi.Controllers
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

        #region List Tickets
        [HttpGet("Ticket-list")]
        public async Task<IActionResult> GetAllTicket()
        {
            var list = await ticketService.GetAllTicket();
            if (list.Any())
            {
                return JsonStatusResponse.Success(message: ("موفق"), ReturnData: list);

            }

            return JsonStatusResponse.NotFound(message: "تیکتی پیدا نشد");
        }

        #endregion

        #region Found Ticket By Id

        [HttpGet("Find-Ticket")]
        public async Task<IActionResult> FindTicket([FromRoute] long ticketId)
        {
            var res = await ticketService.FindTicketById(ticketId);
            if (res != null)
                return JsonStatusResponse.Success(res, "موفق");
            HttpContext.Response.StatusCode = 404;
            return JsonStatusResponse.NotFound("تیکت پیدا نشد ");
        }
        #endregion

        #region Create Tickes
        [HttpPost("Create-Ticket")]
        public async Task<IActionResult> CreateTicket([FromForm] TicketDto ticketDto)
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

        #region Delete Ticket
        [HttpDelete("Delete-Ticket")]
        public async Task<IActionResult> DeleteTicket([FromBody] long ticketId)
        {
            var check = await ticketService.DeleteTicket(ticketId);
            if (check)
                return JsonStatusResponse.Success("تیکت با موفقیت حذف شد");
            return JsonStatusResponse.Error("حذف تیکت با خطا مواجه شد");

        }
        #endregion

        #region Change The Ticket Status
        [HttpGet("Change-Ticket-Status")]
        public async Task<IActionResult> ChangeAdminStatus([FromQuery] long ticketId)
        {
            var res = await ticketService.ChangeTicketStatus(ticketId);
            if (res)
                return JsonStatusResponse.Success("وضعیت تیکت تغییر کرد");
            return JsonStatusResponse.Error("تغییر وضعیت با خطا مواجه شد");
        }
        #endregion
    }
}
