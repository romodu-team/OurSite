using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurSite.Core.DTOs;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Services.Repositories;

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

        [HttpPost("send-form")]
        public async Task<IActionResult> SendContactWithUsForm([FromBody]ContactWithUsDto sendContactForm)
        {
            await contactWithUsService.SendContactForm(sendContactForm);
            return new JsonResult("درخواست شما با موفقیت ارسال شد");
        }
    }
}
