using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurSite.Core.Services.Interfaces.TicketInterfaces;
using OurSite.Core.Utilities;

namespace OurSite.WebApi.Controllers.TicketControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketPriorityController : ControllerBase
    {
        #region Constructor
        private ITicketPriorityService _ticketPriorityService;
        public TicketPriorityController(ITicketPriorityService ticketPriorityService)
        {
            _ticketPriorityService = ticketPriorityService;
        }

        #endregion
        /// <summary>
        /// create a priority for tickets
        /// </summary>
        /// <param name="title"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost("create-ticket-priority")]
        public async Task<IActionResult> CreatePriority([FromQuery]string title,[FromQuery] string name)
        {
            var res = await _ticketPriorityService.CreatePriority(title, name);
            if (res)
                return JsonStatusResponse.Success("ticket Priority has been created successfully");
            return JsonStatusResponse.Error("ticket Priority was not created");
        }
        /// <summary>
        /// delete ticket Priority
        /// </summary>
        /// <param name="PriorityId"></param>
        /// <returns></returns>
        [HttpDelete("Delete-ticket-Priority")]
        public async Task<IActionResult> DeletePriority([FromQuery]long PriorityId)
        {
            var res = await _ticketPriorityService.DeletePriority(PriorityId);
            switch (res)
            {
                case Core.DTOs.TicketDtos.ResDeleteOpration.Success:
                    return JsonStatusResponse.Success("ticket Priority has been deleted successfully");
                case Core.DTOs.TicketDtos.ResDeleteOpration.Failure:
                    return JsonStatusResponse.Error("ticket Priority was not deleted");
                case Core.DTOs.TicketDtos.ResDeleteOpration.RefrenceError:
                    return JsonStatusResponse.Error("You cannot delete this priority because there is an existing ticket with this priority");
                default:
                    return JsonStatusResponse.Error("ticket Priority was not deleted");
            }
            
            
        }
        /// <summary>
        /// get a ticket Priority details
        /// </summary>
        /// <param name="PriorityId"></param>
        /// <returns></returns>
        [HttpGet("Get-ticket-Priority/{PriorityId}")]
        public async Task<IActionResult> GetCategory([FromRoute]long PriorityId)
        {
            var res = await _ticketPriorityService.GetPriority(PriorityId);
            if (res is not null)
                return JsonStatusResponse.Success(res, "successfull");
            return JsonStatusResponse.Error("ticket Priority not found");
        }
        /// <summary>
        /// update ticket Priority , title and name are optional
        /// </summary>
        /// <param name="PriorityId"></param>
        /// <param name="title"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPut("Update-ticket-Priority/{PriorityId}")]
        public async Task<IActionResult> UpdatePriority([FromRoute]long PriorityId,[FromQuery] string? title,[FromQuery] string? name)
        {
            var res = await _ticketPriorityService.UpdatePriority(PriorityId, title, name);
            if (res)
                return JsonStatusResponse.Success("ticket Priority has been updated successfully");
            return JsonStatusResponse.Error("ticket Priority was not updated");
        }
        /// <summary>
        /// get list of ticket Priorities
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all-ticket-Priorities")]
        public async Task<IActionResult> GetAllPriorities()
        {
            var res = await _ticketPriorityService.GetAllPriority();
            if (res is not null && res.Count > 0)
                return JsonStatusResponse.Success(res, "successfull");
            return JsonStatusResponse.Error("no ticket Priority found");
        }
    }
}
