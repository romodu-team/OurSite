using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurSite.Core.DTOs.TicketDtos;
using OurSite.Core.Services.Interfaces.TicketInterfaces;
using OurSite.Core.Utilities;

namespace OurSite.WebApi.Controllers.TicketControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        #region constructor
        private ITicketService _ticketService;
        private ITicketCategoryService _TicketCategoryService;
        private ITicketPriorityService _TicketPriorityService;
        private ITicketStatusService _TicketStatusService;
        public TicketController(ITicketStatusService TicketStatusService, ITicketPriorityService TicketPriorityService, ITicketService ticketService, ITicketCategoryService TicketCategoryService)
        {
            _ticketService = ticketService;
            _TicketPriorityService = TicketPriorityService;
            _TicketCategoryService = TicketCategoryService;
            _TicketStatusService = TicketStatusService;

        }
        #endregion
        /// <summary>
        /// create ticket with optional attachment,user id = ID of the user for whom the ticket will be registered , Sender id= The ID of the person who created the tick and sent the first message , IsAdmin = Set true if the sender is an admin , set false for user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("create-ticket")]
        public async Task<IActionResult> CreateTicket([FromForm]ReqCreateTicketDto request)
        {
            if (ModelState.IsValid)
            {
                var res = await _ticketService.CreateTicket(request);
                switch (res.DiscussionStatus)
                {
                    case ResOperation.notAllowed:
                        return JsonStatusResponse.Error("The ticket status for this operation is not allowed");
                    case ResOperation.NotFound:
                        return JsonStatusResponse.NotFound("ticket not found");
                    case ResOperation.UserNotFound:
                        return JsonStatusResponse.NotFound("User not found");
                    case ResOperation.SenderNotFound:
                        return JsonStatusResponse.NotFound("ticket sender not found");
                    case ResOperation.Success when res.AttachmentStatus == resFileUploader.Success:
                        return JsonStatusResponse.Success("ticket has been created successfully , attachment uploaded");
                    case ResOperation.Success when res.AttachmentStatus == resFileUploader.NoContent:
                        return JsonStatusResponse.Success("ticket has been created successfully , No attachment found");
                    case ResOperation.Failure when res.AttachmentStatus == resFileUploader.Failure:
                        return JsonStatusResponse.Error("server error");
                    default:
                        return JsonStatusResponse.Error("server error while uploading attachment");
                }
            }
            else
                return JsonStatusResponse.InvalidInput();

        }
        /// <summary>
        /// update ticket,Enter the fields you want to update
        /// </summary>
        /// <returns></returns>
        [HttpPut("update-ticket")]
        public async Task<IActionResult> UploadTicket([FromBody]ReqUpdateTicketDto request)
        {
            if (ModelState.IsValid)
            {
                var res = await _ticketService.UpdateTicket(request);
                switch (res)
                {
                    case ResOperation.Success:
                        return JsonStatusResponse.Success("ticket has been updated successfully");
                    case ResOperation.Failure:
                        return JsonStatusResponse.Error("server error");
                    case ResOperation.NotFound:
                        return JsonStatusResponse.NotFound("ticket not found");
                    default:
                        return JsonStatusResponse.UnhandledError();
                }
            }
            else
                return JsonStatusResponse.InvalidInput();
        }
        /// <summary>
        /// delete ticket with related discussion and attachments
        /// </summary>
        /// <param name="TicketId"></param>
        /// <returns></returns>
        [HttpDelete("delete-ticket")]
        public async Task<IActionResult> DeleteTicket([FromQuery]long TicketId)
        {
            var res = await _ticketService.DeleteTicket(TicketId);
            switch (res)
            {
                case ResOperation.Success:
                    return JsonStatusResponse.Success("ticket has been deleted successfully");
                case ResOperation.Failure:
                    return JsonStatusResponse.Error("server error");
                default:
                    return JsonStatusResponse.UnhandledError();
            }
        }
        /// <summary>
        /// change ticket status
        /// </summary>
        /// <param name="TicketId"></param>
        /// <param name="StatusId"></param>
        /// <returns></returns>
        [HttpPut("change-ticket-status")]
        public async Task<IActionResult> ChangeTicketStatus([FromQuery]long TicketId, [FromQuery]long StatusId)
        {
            var res =await _ticketService.ChangeTicketStatus(TicketId, StatusId);
            switch (res)
            {
                case ResOperation.Success:
                    return JsonStatusResponse.Success("Ticket status successfully edited");
                case ResOperation.Failure:
                    return JsonStatusResponse.Error("server error");
                case ResOperation.StatusNotFound:
                    return JsonStatusResponse.NotFound("status not found");
                case ResOperation.NotFound:
                    return JsonStatusResponse.NotFound("ticket not found");

                default:
                    return JsonStatusResponse.UnhandledError();
            }
        }
        /// <summary>
        /// Getting the list of tickets assigned to an admin
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("Get-Ticket-Assigned-To-Admin")]
        public async Task<IActionResult> GetTicketAssignedToAdmin([FromQuery]long adminId,[FromQuery] ReqGetAllTicketDto request)
        {
            var res =await _ticketService.GetTicketAssignedToAdmin(adminId, request);
            if (res.Tickets != null)
                return JsonStatusResponse.Success(res, "successfully");
            return JsonStatusResponse.NotFound("No tickets found");
        }
        /// <summary>
        /// Getting the list of tickets assigned to an User
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("get-User-tickets")]
        public async Task<IActionResult> GetUserTickets([FromQuery]long UserId,[FromQuery] ReqGetAllUserTicketsDto request)
        {
            var res = await _ticketService.GetUserTickets(UserId, request);
            if (res.Tickets is not null && res.Tickets.Count>0)
                return JsonStatusResponse.Success(res, "successfully");
            return JsonStatusResponse.NotFound("No tickets found");
        }
        /// <summary>
        /// Getting a list of all tickets
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("get-all-tickets")]
        public async Task<IActionResult> GetAllTickets([FromQuery]ReqGetAllTicketDto request)
        {
            var res = await _ticketService.GetAllTickets(request);
            if (res.Tickets != null && res.Tickets.Count>0)
                return JsonStatusResponse.Success(res, "successfully");
            return JsonStatusResponse.NotFound("No tickets found");
        }
        /// <summary>
        /// get a ticket details
        /// </summary>
        /// <param name="TicketId"></param>
        /// <returns></returns>
        [HttpGet("get-ticket/{TicketId}")]
        public async Task<IActionResult> GetTicket([FromRoute]long TicketId)
        {
            var res =await _ticketService.GetTicket(TicketId);
            if (res is not null)
                return JsonStatusResponse.Success(res, "Success");
            else
                return JsonStatusResponse.NotFound("Ticket not found");
        }
        /// <summary>
        /// Getting reports about tickets
        /// </summary>
        /// <returns></returns>
        [HttpGet("ticket-reports")]
        public async Task<IActionResult> TicketReport()
        {
            var res = await _ticketService.TicketReport();
            if (res is not null)
                return JsonStatusResponse.Success(res, "Success");
            else
                return JsonStatusResponse.NotFound("ticket not found");
        }
        /// <summary>
        /// create a discussion for a ticket
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("create-discussion")]
        public async Task<IActionResult> CreateDiscussion([FromForm]ReqCreateDiscussion request)
        {
            
            if (ModelState.IsValid)
            {
                var res = await _ticketService.CreateDiscussion(request);
                switch (res.DiscussionStatus)
                {
                    case ResOperation.notAllowed:
                        return JsonStatusResponse.Error("The ticket status for this operation is not allowed");
                    case ResOperation.NotFound:
                        return JsonStatusResponse.NotFound("ticket not found");
                    case ResOperation.UserNotFound:
                        return JsonStatusResponse.NotFound("User not found");
                    case ResOperation.SenderNotFound:
                        return JsonStatusResponse.NotFound("ticket sender not found");
                    case ResOperation.Success when res.AttachmentStatus == resFileUploader.Success:
                        return JsonStatusResponse.Success("discussion has been created successfully , attachment uploaded");
                    case ResOperation.Success when res.AttachmentStatus == resFileUploader.NoContent:
                        return JsonStatusResponse.Success("discussion has been created successfully , No attachment found");
                    case ResOperation.Failure when res.AttachmentStatus == resFileUploader.Failure:
                        return JsonStatusResponse.Error("server error");
                    default:
                        return JsonStatusResponse.Error("server error while uploading attachment");
                }
            }
            else
                return JsonStatusResponse.InvalidInput();
        }
        /// <summary>
        /// Delete multiple discussions
        /// </summary>
        /// <param name="DiscussionsId"></param>
        /// <returns></returns>
        [HttpDelete("delete-discussion")]
        public async Task<IActionResult> DeleteDiscussion([FromBody]List<long> DiscussionsId)
        {
            var res = await _ticketService.DeleteDiscussion(DiscussionsId);
            switch (res)
            {
                case ResOperation.Success:
                    return JsonStatusResponse.Success("discussions has been deleted successfully");
                case ResOperation.Failure:
                    return JsonStatusResponse.Error("server error");
                default:
                    return JsonStatusResponse.UnhandledError();
            }
        }
    }
}
