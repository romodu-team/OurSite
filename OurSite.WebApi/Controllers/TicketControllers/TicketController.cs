using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OurSite.Core.DTOs.TicketDtos;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Services.Interfaces.TicketInterfaces;
using OurSite.Core.Services.Repositories;
using OurSite.Core.Utilities;
using OurSite.DataLayer.Entities.NotificationModels;
using OurSite.DataLayer.Interfaces;

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
        public TicketController( ITicketStatusService TicketStatusService, ITicketPriorityService TicketPriorityService, ITicketService ticketService, ITicketCategoryService TicketCategoryService)
        {
            _ticketService = ticketService;
            _TicketPriorityService = TicketPriorityService;
            _TicketCategoryService = TicketCategoryService;
            _TicketStatusService = TicketStatusService;
        }
        #endregion
        /// <summary>
        /// create ticket by admin with optional attachment,user id = ID of the user for whom the ticket will be registered , Sender id= The ID of the person who created the tick and sent the first message
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Admin-create-ticket")]
        [Authorize(Policy =StaticPermissions.PermissionCreateTicket)]
        public async Task<IActionResult> AdminCreateTicket([FromForm] ReqCreateTicketDto request)
        {
            if (ModelState.IsValid)
            {
                var res = await _ticketService.CreateTicket(request,true);
                switch (res.DiscussionStatus)
                {
                    case ResOperation.notAllowed:
                        HttpContext.Response.StatusCode = 403;
                        return JsonStatusResponse.Error("The ticket status for this operation is not allowed");
                    case ResOperation.NotFound:
                        HttpContext.Response.StatusCode = 404;
                        return JsonStatusResponse.NotFound("ticket not found");
                    case ResOperation.UserNotFound:
                        HttpContext.Response.StatusCode = 404;
                        return JsonStatusResponse.NotFound("User not found");
                    case ResOperation.SenderNotFound:
                        HttpContext.Response.StatusCode = 404;
                        return JsonStatusResponse.NotFound("ticket sender not found");
                    case ResOperation.Success when res.AttachmentStatus == resFileUploader.Success:
                        HttpContext.Response.StatusCode = 201;
                        return JsonStatusResponse.Success(message: "ticket has been created successfully , attachment uploaded", ReturnData: request);
                    case ResOperation.Success when res.AttachmentStatus == resFileUploader.NoContent:
                        HttpContext.Response.StatusCode = 201;
                        return JsonStatusResponse.Success(message: "ticket has been created successfully , No attachment found", ReturnData: request);
                    case ResOperation.Failure when res.AttachmentStatus == resFileUploader.Failure:
                        HttpContext.Response.StatusCode = 500;
                        return JsonStatusResponse.Error("server error");
                    default:
                        HttpContext.Response.StatusCode = 500;
                        return JsonStatusResponse.UnhandledError();
                }
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
                return JsonStatusResponse.InvalidInput();
            }

        }
        /// <summary>
        /// create ticket by user with optional attachment,user id = ID of the user for whom the ticket will be registered , Sender id= The ID of the person who created the tick and sent the first message
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User-create-ticket")]
        [Authorize]
        public async Task<IActionResult> UserCreateTicket([FromForm] ReqCreateTicketDto request)
        {
            if (ModelState.IsValid)
            {
                var res = await _ticketService.CreateTicket(request,false);
                switch (res.DiscussionStatus)
                {
                    case ResOperation.notAllowed:
                        HttpContext.Response.StatusCode = 403;
                        return JsonStatusResponse.Error("The ticket status for this operation is not allowed");
                    case ResOperation.NotFound:
                        HttpContext.Response.StatusCode = 404;
                        return JsonStatusResponse.NotFound("ticket not found");
                    case ResOperation.UserNotFound:
                        HttpContext.Response.StatusCode = 404;
                        return JsonStatusResponse.NotFound("User not found");
                    case ResOperation.SenderNotFound:
                        HttpContext.Response.StatusCode = 404;
                        return JsonStatusResponse.NotFound("ticket sender not found");
                    case ResOperation.Success when res.AttachmentStatus == resFileUploader.Success:
                        HttpContext.Response.StatusCode = 201;
                        return JsonStatusResponse.Success(message: "ticket has been created successfully , attachment uploaded", ReturnData: request);
                    case ResOperation.Success when res.AttachmentStatus == resFileUploader.NoContent:
                        HttpContext.Response.StatusCode = 201;
                        return JsonStatusResponse.Success(message: "ticket has been created successfully , No attachment found", ReturnData: request);
                    case ResOperation.Failure when res.AttachmentStatus == resFileUploader.Failure:
                        HttpContext.Response.StatusCode = 500;
                        return JsonStatusResponse.Error("server error");
                    default:
                        HttpContext.Response.StatusCode = 500;
                        return JsonStatusResponse.UnhandledError();
                }
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
                return JsonStatusResponse.InvalidInput();
            }

        }

        /// <summary>
        /// update ticket,Enter the fields you want to update
        /// </summary>
        /// <returns></returns>
        [HttpPut("update-ticket")]
        [Authorize(Policy = StaticPermissions.PermissionUpdateTicket)]

        public async Task<IActionResult> UploadTicket([FromBody] ReqUpdateTicketDto request)
        {
            if (ModelState.IsValid)
            {
                var res = await _ticketService.UpdateTicket(request);
                switch (res)
                {
                    case ResOperation.Success:
                        HttpContext.Response.StatusCode = 200;
                        return JsonStatusResponse.Success("ticket has been updated successfully");
                    case ResOperation.Failure:
                        HttpContext.Response.StatusCode = 500;
                        return JsonStatusResponse.Error("ticket has not been updated");
                    case ResOperation.NotFound:
                        HttpContext.Response.StatusCode = 404;
                        return JsonStatusResponse.NotFound("ticket not found");
                    default:
                        HttpContext.Response.StatusCode = 500;
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
        [Authorize(Policy = StaticPermissions.PermissionDeleteTicket)]

        public async Task<IActionResult> DeleteTicket([FromQuery] long TicketId)
        {
            var res = await _ticketService.DeleteTicket(TicketId);
            switch (res)
            {
                case ResOperation.Success:
                    HttpContext.Response.StatusCode = 200;
                    return JsonStatusResponse.Success(message: "ticket has been deleted successfully" , ReturnData: TicketId);
                case ResOperation.Failure:
                    HttpContext.Response.StatusCode = 500;
                    return JsonStatusResponse.Error("server error");
                default:
                    HttpContext.Response.StatusCode = 500;
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
        [Authorize(Policy = StaticPermissions.PermissionChangeTicketStatus)]

        public async Task<IActionResult> ChangeTicketStatus([FromQuery] long TicketId, [FromQuery] long StatusId)
        {
            var res = await _ticketService.ChangeTicketStatus(TicketId, StatusId);
            switch (res)
            {
                case ResOperation.Success:
                    HttpContext.Response.StatusCode = 200;
                    return JsonStatusResponse.Success(message : "Ticket status successfully edited" , ReturnData: TicketId);
                case ResOperation.Failure:
                    HttpContext.Response.StatusCode = 500;
                    return JsonStatusResponse.Error("server error");
                case ResOperation.StatusNotFound:
                    HttpContext.Response.StatusCode = 404;
                    return JsonStatusResponse.NotFound("status not found");
                case ResOperation.NotFound:
                    HttpContext.Response.StatusCode = 404;
                    return JsonStatusResponse.NotFound("ticket not found");

                default:
                    HttpContext.Response.StatusCode = 500;
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
        [Authorize]
        public async Task<IActionResult> GetTicketAssignedToAdmin([FromQuery] long adminId, [FromQuery] ReqGetAllTicketDto request)
        {
            var res = await _ticketService.GetTicketAssignedToAdmin(adminId, request);
            if (res.Tickets != null)
            {
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success(res, "successfully");
            }
            HttpContext.Response.StatusCode = 404;
            return JsonStatusResponse.NotFound("No tickets found");
        }
        /// <summary>
        /// Getting the list of tickets assigned to an User
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("get-User-tickets")]
        [Authorize]
        public async Task<IActionResult> GetUserTickets([FromQuery] long UserId, [FromQuery] ReqGetAllUserTicketsDto request)
        {
            var res = await _ticketService.GetUserTickets(UserId, request);
            if (res.Tickets is not null && res.Tickets.Count > 0)
            {
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success(res, "successfully");
            }
            HttpContext.Response.StatusCode = 404;
            return JsonStatusResponse.NotFound("No tickets found");
        }
        /// <summary>
        /// Getting a list of all tickets
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("get-all-tickets")]
        [Authorize(Policy =StaticPermissions.PermissionViewAllTickets)]
        public async Task<IActionResult> GetAllTickets([FromQuery] ReqGetAllTicketDto request)
        {
            var res = await _ticketService.GetAllTickets(request);
            if (res.Tickets != null && res.Tickets.Count > 0)
            {
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success(res, "successfully");
            }
            HttpContext.Response.StatusCode = 404;
            return JsonStatusResponse.NotFound("No tickets found");
        }
        /// <summary>
        /// get a ticket details
        /// </summary>
        /// <param name="TicketId"></param>
        /// <returns></returns>
        [HttpGet("get-ticket/{TicketId}")]
        [Authorize]
        public async Task<IActionResult> GetTicket([FromRoute] long TicketId)
        {
            var res = await _ticketService.GetTicket(TicketId);
            if (res is not null)
            {
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success(res, "Success");
            }
            HttpContext.Response.StatusCode = 404;
            return JsonStatusResponse.NotFound("Ticket not found");
        }
        /// <summary>
        /// Getting reports about tickets
        /// </summary>
        /// <returns></returns>
        [HttpGet("ticket-reports")]
        [Authorize]
        public async Task<IActionResult> TicketReport()
        {
            var res = await _ticketService.TicketReport();
            if (res is not null)
            {
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success(res, "Success");
            }
            HttpContext.Response.StatusCode = 404;
            return JsonStatusResponse.NotFound("ticket not found");
        }
        /// <summary>
        /// create a discussion for a ticket
        /// </summary>
        /// <remarks>The size of the attached file must be less than 20 MB</remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("create-discussion")]
        [Authorize]
        public async Task<IActionResult> CreateDiscussion([FromForm] ReqCreateDiscussion request)
        {

            if (ModelState.IsValid)
            {
                var res = await _ticketService.CreateDiscussion(request);
                switch (res.DiscussionStatus)
                {
                    case ResOperation.notAllowed:
                        HttpContext.Response.StatusCode = 400;
                        return JsonStatusResponse.Error("The ticket status for this operation is not allowed");
                    case ResOperation.NotFound:
                        HttpContext.Response.StatusCode = 404;
                        return JsonStatusResponse.NotFound("ticket not found");
                    case ResOperation.UserNotFound:
                        HttpContext.Response.StatusCode = 404;
                        return JsonStatusResponse.NotFound("User not found");
                    case ResOperation.SenderNotFound:
                        HttpContext.Response.StatusCode = 404;
                        return JsonStatusResponse.NotFound("ticket sender not found");
                    case ResOperation.Success when res.AttachmentStatus == resFileUploader.Success:
                        HttpContext.Response.StatusCode = 201;
                        return JsonStatusResponse.Success(message:"discussion has been created successfully , attachment uploaded" , ReturnData: request);
                    case ResOperation.Success when res.AttachmentStatus == resFileUploader.NoContent:
                        HttpContext.Response.StatusCode = 201;
                        return JsonStatusResponse.Success(message:"discussion has been created successfully , No attachment found" , ReturnData: request);
                    case ResOperation.Failure when res.AttachmentStatus == resFileUploader.Failure:
                        HttpContext.Response.StatusCode = 500;
                        return JsonStatusResponse.Error("server error");
                    default:
                        HttpContext.Response.StatusCode = 500;
                        return JsonStatusResponse.UnhandledError();
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
        [Authorize(Policy =StaticPermissions.PermissionDeleteDiscussion)]
        public async Task<IActionResult> DeleteDiscussion([FromBody] List<long> DiscussionsId)
        {
            var res = await _ticketService.DeleteDiscussion(DiscussionsId);
            switch (res)
            {
                case ResOperation.Success:
                    HttpContext.Response.StatusCode = 200;
                    return JsonStatusResponse.Success("discussions has been deleted successfully");
                case ResOperation.Failure:
                    HttpContext.Response.StatusCode = 500;
                    return JsonStatusResponse.Error("server error");
                default:
                    HttpContext.Response.StatusCode = 500;
                    return JsonStatusResponse.UnhandledError();
            }
        }
    }
}
