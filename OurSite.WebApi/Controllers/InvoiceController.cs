using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurSite.Core.DTOs.InvoiceDtos;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Utilities;

namespace OurSite.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        #region constructor

        private IInvoiceService invoiceService;
        public InvoiceController(IInvoiceService invoiceService)
        {
            this.invoiceService = invoiceService;
        }
        #endregion


        #region Create Factor
        [HttpPost("Create-Factor")]
        public async Task<IActionResult> CreateFactor([FromForm] InvoiceDto invoiceDto)
        {

            var res = await invoiceService.createFactor(invoiceDto);

            switch (res.resInvoice)
            {
                case ResInvoice.Success:
                    return JsonStatusResponse.Success("فاکتور با موفقیت ثبت شد");

                default:
                    return JsonStatusResponse.Error("ثبت فاکتور با خطا مواجه شد");
            }
        }
        #endregion

    }
}
