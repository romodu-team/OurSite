using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurSite.Core.Services.Interfaces.TicketInterfaces;
using OurSite.Core.Utilities;

namespace OurSite.WebApi.Controllers.TicketControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketStatusController : ControllerBase
    {
        #region Constructor
        private ITicketStatusService _ticketStatusService;
        public TicketStatusController(ITicketStatusService ticketStatusService)
        {
            _ticketStatusService = ticketStatusService;
        }

        #endregion
        /// <summary>
        /// create a Status for tickets
        /// </summary>
        /// <param name="title"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost("create-ticket-Status")]
        public async Task<IActionResult> CreateStatus([FromQuery] string title, [FromQuery] string name)
        {
            var res = await _ticketStatusService.CreateStatus(title, name);
            if (res)
                return JsonStatusResponse.Success(message:"ticket Status has been created successfully" , ReturnData: title);
            return JsonStatusResponse.Error("ticket Status was not created");
        }
        /// <summary>
        /// delete ticket Status
        /// </summary>
        /// <param name="StatusId"></param>
        /// <returns></returns>
        [HttpDelete("Delete-ticket-Status")]
        public async Task<IActionResult> DeleteStatus([FromQuery] long StatusId)
        {
            var res = await _ticketStatusService.DeleteStatus(StatusId);
            switch (res)
            {
                case Core.DTOs.TicketDtos.ResDeleteOpration.Success:
                    return JsonStatusResponse.Success(message:"ticket Status has been deleted successfully" , ReturnData: StatusId);
                case Core.DTOs.TicketDtos.ResDeleteOpration.Failure:
                    return JsonStatusResponse.Error("ticket Status was not deleted");
                case Core.DTOs.TicketDtos.ResDeleteOpration.RefrenceError:
                    return JsonStatusResponse.Error("You cannot delete this Status because there is an existing ticket with this Status");
                default:
                    return JsonStatusResponse.Error("ticket Status was not deleted");
            }
        }
        /// <summary>
        /// get a ticket Priority details
        /// </summary>
        /// <param name="StatusId"></param>
        /// <returns></returns>
        [HttpGet("Get-ticket-Status/{StatusId}")]
        public async Task<IActionResult> GetStatus([FromRoute] long StatusId)
        {
            var res = await _ticketStatusService.GetStatus(StatusId);
            if (res is not null)
                return JsonStatusResponse.Success(res, "successfull");
            return JsonStatusResponse.Error("ticket Status not found");
        }
        /// <summary>
        /// update ticket Status , title and name are optional
        /// </summary>
        /// <param name="StatusId"></param>
        /// <param name="title"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPut("Update-ticket-Status/{StatusId}")]
        public async Task<IActionResult> UpdateStatus([FromRoute] long StatusId, [FromQuery] string? title, [FromQuery] string? name)
        {
            var res = await _ticketStatusService.UpdateStatus(StatusId, title, name);
            if (res)
                return JsonStatusResponse.Success(message: "ticket Status has been updated successfully" , ReturnData: StatusId);
            return JsonStatusResponse.Error("ticket Status was not updated");
        }
        /// <summary>
        /// get list of ticket Status
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all-ticket-Status")]
        public async Task<IActionResult> GetAllStatus()
        {
            var res = await _ticketStatusService.GetAllStatus();
            if (res is not null && res.Count > 0)
                return JsonStatusResponse.Success(res, "successfull");
            return JsonStatusResponse.Error("no ticket Status found");
        }
    }
}
