using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurSite.Core.DTOs.ContactWithUsDtos;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Services.Repositories;
using OurSite.Core.Utilities;

namespace OurSite.WebApi.Controllers.Forms
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactWithUsController : ControllerBase
    {
        private IContactWithUsService contactWithUsService;

        public ContactWithUsController(IContactWithUsService contactWithUsService)
        {
            this.contactWithUsService = contactWithUsService;
        }

        #region Send ContactWithUs

        /// <summary>
        /// Send contact with us form {Get request from form}
        /// Returns : Success , Error
        /// </summary>
        /// <param name="sendContactForm"></param>
        /// <returns></returns>
        [HttpPost("send-contact-us")]
        public async Task<IActionResult> SendContactWithUsForm([FromForm] ContactWithUsDto sendContactForm)
        {
            if (ModelState.IsValid)
            {
                await contactWithUsService.SendContactForm(sendContactForm);
                return JsonStatusResponse.Success("درخواست شما با موفقیت ارسال شد");
            }
            else
            {
                return JsonStatusResponse.Error("اطلاعات ارسال شده معتبر نمی‌باشد");
            }
   

        }
        #endregion

        #region ContactWithUs All Form
        /// <summary>
        /// Get all Contact With Us forms filtered by pagination
        /// Returns: Success with Return Data ,NotFound
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("view-all-ContactWithUs")]
        public async Task<IActionResult> GetAllContactWithUs([FromQuery] ReqFilterContactWithUsDto filter)
        {
            var contactWithUs = await contactWithUsService.GetAllContactWithUs(filter);
            if (contactWithUs.ContactWithUses is not null)
            {
                return JsonStatusResponse.Success(message: "موفق", ReturnData: contactWithUs);
            }
            return JsonStatusResponse.NotFound(message: "فرمی یافت نشد");

        }
        #endregion
    }
}
