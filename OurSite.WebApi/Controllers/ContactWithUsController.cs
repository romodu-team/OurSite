using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurSite.Core.DTOs.ContactWithUsDtos;
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

        #region Send ContactWithUs
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
        }
        #endregion

        #region ContactWithUs All Form
        [HttpGet("view-all-ContactWithUs")] //Get user list
        public async Task<IActionResult> GetAllContactWithUs([FromQuery] ReqFilterContactWithUsDto filter)
        {
            var contactWithUs = await contactWithUsService.GetAllContactWithUs(filter);
            if (contactWithUs.ContactWithUses.Any())
            {
                return JsonStatusResponse.Success(message: "موفق", ReturnData: contactWithUs);
            }
            return JsonStatusResponse.NotFound(message: "فرمی یافت نشد");

        }
        #endregion
    }
}
