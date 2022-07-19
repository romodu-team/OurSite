using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OurSite.Core.DTOs.Payment;
using System.Text;

namespace OurSite.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private static readonly HttpClient client = new HttpClient();

        [HttpPost]
        public async Task<IActionResult> RequestPayment(PaymentRequestDto paymentRequestDto)
        {
            var _url = "https://sandbox.zarinpal.com/pg/rest/WebGate/PaymentRequest.json";

            var _values = new Dictionary<string, string>
            {
               { "MerchantID", "00000000-0000-0000-0000-000000000000" }, //Change This To work, some thing like this : xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
               { "Amount", paymentRequestDto.amount }, //Toman
               { "CallbackURL", $"http://localhost:7181/api/payment/VerifyPayment?amount={paymentRequestDto.amount}" },
               { "Mobile", paymentRequestDto.phoneNumber }, //Mobile number will be shown in the transactions list of the wallet as a separate field.
               { "Description", paymentRequestDto.description }
            };

            var _paymentRequestJsonValue = JsonConvert.SerializeObject(_values);
            var content = new StringContent(_paymentRequestJsonValue, Encoding.UTF8, "application/json");

            var _response = await client.PostAsync(_url, content);
            var _responseString = await _response.Content.ReadAsStringAsync();

            ZarinPalRequestResponse _zarinPalResponseModel =
             JsonConvert.DeserializeObject<ZarinPalRequestResponse>(_responseString);


            var finalUrl = $"https://sandbox.zarinpal.com/pg/StartPay/{_zarinPalResponseModel.Authority/*+"/Sad"*/}";
           return Ok(finalUrl); 
            
        }

        [HttpGet("VerifyPayment")]
        public async Task<IActionResult> VerifyPayment(string Authority, PaymentRequestDto paymentRequestDto)
        {
            var _url = "https://sandbox.zarinpal.com/pg/rest/WebGate/PaymentVerification.json";

            var _values = new Dictionary<string, string>
            {
               { "MerchantID", "00000000-0000-0000-0000-000000000000" }, //Change This To work, some thing like this : xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
               { "Authority", Authority },
               { "Amount", paymentRequestDto.amount } //Toman
            };

            var _paymenResponsetJsonValue = JsonConvert.SerializeObject(_values);
            var content = new StringContent(_paymenResponsetJsonValue, Encoding.UTF8, "application/json");

            var _response = await client.PostAsync(_url, content);
            var _responseString = await _response.Content.ReadAsStringAsync();


            ZarinPalVerifyResponse _zarinPalResponseModel =
             JsonConvert.DeserializeObject<ZarinPalVerifyResponse>(_responseString);


            return Ok(_zarinPalResponseModel);
        }
    }
}
