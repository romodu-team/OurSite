using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurSite.Core.Services.Interfaces.TicketInterfaces;
using OurSite.Core.Utilities;

namespace OurSite.WebApi.Controllers.TicketControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = StaticPermissions.PermissionManageTicketCategory)]
    public class TicketCategoryController : ControllerBase
    {
        #region Constructor
        private ITicketCategoryService _ticketCategoryService;
        public TicketCategoryController(ITicketCategoryService ticketCategoryService)
        {
            _ticketCategoryService = ticketCategoryService;
        }

        #endregion
       
        /// <summary>
        /// create a category for tickets
        /// </summary>
        /// <param name="title"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost("create-ticket-category")]
        public async Task<IActionResult> CreateCategory(string title, string name)
        {
            var res = await _ticketCategoryService.CreateCategory(title, name);
            if (res)
            {
                HttpContext.Response.StatusCode = 201;
                return JsonStatusResponse.Success(message: "ticket category has been created successfully", ReturnData: title);
            }
            HttpContext.Response.StatusCode = 500;
            return JsonStatusResponse.Error("ticket category was not created");
        }




        /// <summary>
        /// delete ticket category
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        [HttpDelete("Delete-ticket-Category")]
        public async Task<IActionResult> DeleteCategory(long CategoryId)
        {
            var res = await _ticketCategoryService.DeleteCategory(CategoryId);
            switch (res)
            {
                case Core.DTOs.TicketDtos.ResDeleteOpration.Success:
                    HttpContext.Response.StatusCode = 200;
                    return JsonStatusResponse.Success(message:"ticket category has been deleted successfully", ReturnData: CategoryId);
                case Core.DTOs.TicketDtos.ResDeleteOpration.Failure:
                    HttpContext.Response.StatusCode = 500;
                    return JsonStatusResponse.Error("ticket category was not deleted");
                case Core.DTOs.TicketDtos.ResDeleteOpration.RefrenceError:
                    HttpContext.Response.StatusCode = 409;
                    return JsonStatusResponse.Error("You cannot delete this category because there is an existing ticket with this category");
                default:
                    HttpContext.Response.StatusCode = 500;
                    return JsonStatusResponse.UnhandledError();
            }
        }
        /// <summary>
        /// get a ticket category details
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        [HttpGet("Get-ticket-Category")]
        public async Task<IActionResult> GetCategory(long CategoryId)
        {
            var res = await _ticketCategoryService.GetCategory(CategoryId);
            if (res is not null)
            {
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success(res, "successfull");
            }
            HttpContext.Response.StatusCode = 404;
            return JsonStatusResponse.Error("ticket category not found");
        }
        /// <summary>
        /// update ticket category , title,name and parentId are optional
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <param name="title"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPut("Update-ticket-Category")]
        public async Task<IActionResult> UpdateCategory(long CategoryId, string? title, string? name,long? parentId)
        {
            var res = await _ticketCategoryService.UpdateCategory(CategoryId,title, name,parentId);
            if (res)
            {
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success(message: "ticket category has been updated successfully", ReturnData: CategoryId);
            }
            HttpContext.Response.StatusCode = 500;
            return JsonStatusResponse.Error("ticket category was not updated");
        }
        /// <summary>
        /// get list of ticket categories
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all-ticket-categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var res = await _ticketCategoryService.GetAllCategories();
            if (res is not null && res.Count > 0)
            {
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success(res, "successfull");
            }
            HttpContext.Response.StatusCode = 404;
            return JsonStatusResponse.Error("no ticket category found");
        }
    }
}
