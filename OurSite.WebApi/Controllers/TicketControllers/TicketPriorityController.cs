using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurSite.Core.Services.Interfaces.TicketInterfaces;
using OurSite.Core.Utilities;

namespace OurSite.WebApi.Controllers.TicketControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = StaticPermissions.PermissionManageTicketPriority)]

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
            {
                HttpContext.Response.StatusCode = 201;
                return JsonStatusResponse.Success(message: "ticket Priority has been created successfully", ReturnData: title);
            }
            HttpContext.Response.StatusCode = 500;
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
                    HttpContext.Response.StatusCode = 200;
                    return JsonStatusResponse.Success(message:"ticket Priority has been deleted successfully" , ReturnData: PriorityId);
                case Core.DTOs.TicketDtos.ResDeleteOpration.Failure:
                    HttpContext.Response.StatusCode = 500;
                    return JsonStatusResponse.Error("ticket Priority was not deleted");
                case Core.DTOs.TicketDtos.ResDeleteOpration.RefrenceError:
                    HttpContext.Response.StatusCode = 409;
                    return JsonStatusResponse.Error("You cannot delete this priority because there is an existing ticket with this priority");
                default:
                    HttpContext.Response.StatusCode = 500;
                    return JsonStatusResponse.UnhandledError();
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
            {
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success(res, "successfull");
            }
            HttpContext.Response.StatusCode = 404;
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
            {
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success(message: "ticket Priority has been updated successfully", ReturnData: PriorityId);
            }
            HttpContext.Response.StatusCode = 500;
            return JsonStatusResponse.UnhandledError();
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
            {
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success(res, "successfull");
            }
            HttpContext.Response.StatusCode = 404;
            return JsonStatusResponse.Error("no ticket Priority found");
        }
    }
}
