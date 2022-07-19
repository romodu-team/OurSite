using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurSite.Core.DTOs;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Services.Repositories;
using OurSite.Core.Utilities;

namespace OurSite.WebApi.Controllers
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

        #region form
        /// <summary>
        ///  form contact with us {Get request from form}
        /// </summary>
        /// <param name="sendContactForm"></param>
        /// <returns></returns>
        [HttpPost("send-form")]
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
            #endregion

        }
    }
}
