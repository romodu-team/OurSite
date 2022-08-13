using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OurSite.Core.DTOs.Payment;
using OurSite.Core.Services.Interfaces.Projecta;
using OurSite.Core.Utilities;
using System.Security.Claims;
using System.Text;
using static OurSite.Core.DTOs.ProjectDtos.CreateProjectDto;

namespace OurSite.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private static readonly HttpClient client = new HttpClient();
        private IPayment Paymentservice;
        public PaymentController(IPayment paymentservice)
        {
            this.Paymentservice = paymentservice;
        }


        #region Req Payment
        /// <summary>
        /// Submit a payment request to the app
        /// , return The link of the payment gateway to which the user should be redirected
        /// </summary>
        /// <param name="paymentRequestDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RequestPayment([FromBody] PaymentRequestDto paymentRequestDto)
        {
            var _url = "https://sandbox.zarinpal.com/pg/rest/WebGate/PaymentRequest.json";
      
            var _values = new Dictionary<string, string>
            {
               { "MerchantID", "00000000-0000-0000-0000-000000000000" }, //Change This To work, some thing like this : xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
               { "Amount", paymentRequestDto.amount }, //Toman
               { "CallbackURL", $"https://localhost:7181/api/payment/VerifyPayment?amount={paymentRequestDto.amount}" },
               { "Mobile", paymentRequestDto.phoneNumber }, //Mobile number will be shown in the transactions list of the wallet as a separate field.
               { "Description", paymentRequestDto.description }
            };

            var _paymentRequestJsonValue = JsonConvert.SerializeObject(_values);
            var content = new StringContent(_paymentRequestJsonValue, Encoding.UTF8, "application/json");

            var _response = await client.PostAsync(_url, content);
            var _responseString = await _response.Content.ReadAsStringAsync();

            ZarinPalRequestResponse _zarinPalResponseModel =JsonConvert.DeserializeObject<ZarinPalRequestResponse>(_responseString);
            _zarinPalResponseModel.Amount = paymentRequestDto.amount;
           
            var finalUrl = $"https://sandbox.zarinpal.com/pg/StartPay/{_zarinPalResponseModel.Authority/*+"/Sad"*/}";
            _zarinPalResponseModel.GateWayUrl = finalUrl;
            
            return Ok(_zarinPalResponseModel);

        }
        #endregion

        #region Verify
        /// <summary>
        /// This function should be executed when returning from the payment gateway. This function validates the user's payment
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="Authority"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        [HttpGet("VerifyPayment")]
        public async Task<IActionResult> VerifyPayment([FromQuery] string amount,[FromQuery] string Authority, [FromQuery]string Status)
        {
            var _url = "https://sandbox.zarinpal.com/pg/rest/WebGate/PaymentVerification.json";

            var _values = new Dictionary<string, string>
            {
               { "MerchantID", "00000000-0000-0000-0000-000000000000" }, //Change This To work, some thing like this : xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
               { "Authority", Authority },
               { "Amount", amount }, //Toman
                {"Status",Status }
            };
            var result = "";
            _values.TryGetValue("Status", out result);
            if (result == "OK")
            {
                var _paymenResponsetJsonValue = JsonConvert.SerializeObject(_values);
                var content = new StringContent(_paymenResponsetJsonValue, Encoding.UTF8, "application/json");

                var _response = await client.PostAsync(_url, content);
                var _responseString = await _response.Content.ReadAsStringAsync();


                ZarinPalVerifyResponse _zarinPalResponseModel =
                 JsonConvert.DeserializeObject<ZarinPalVerifyResponse>(_responseString);

                if (_zarinPalResponseModel.Status == 100)
                    return JsonStatusResponse.Success(_zarinPalResponseModel, "Payment was successful");
                else
                    return JsonStatusResponse.Error("The transaction has already been verified");
            }
            else
            {
                return JsonStatusResponse.Error(message: "Transaction failed");
            }
                
            
        }
        #endregion

        #region Creat Payment  by admin
        /// <summary>
        /// Api for creat payment by admin {Get request from body}
        /// </summary>
        /// <param name="paydto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("create-payment-by-admin")]
         public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDto prodto)
         {
                var res = await Paymentservice.CreatePayment(prodto);
                switch (res)
                {
                    case ResProject.Success:
                        return JsonStatusResponse.Success("پروژه با موفقیت ایجاد شد.");
                    case ResProject.Faild:
                        return JsonStatusResponse.Error("ایجاد پروژه با خطا مواجه شد.");
                    case ResProject.InvalidInput:
                        return JsonStatusResponse.Error("فیلد‌های ثبت پروژه نمی‌تواند خالی باشد.");
                case ResProject.NotFound:
                    return JsonStatusResponse.NotFound("project to creat payment not found");
                    default:
                        return JsonStatusResponse.Error("ثبت پروژه با خطا مواجه شد. دقایقی دیگر مجدد تلاش نمایید.");

                }
                return JsonStatusResponse.Error("U must login");

         }


        #endregion



        #region get payment
        /// <summary>
        /// Api for show one payment to user {get request from route}
        /// </summary>
        /// <param name="PayId"></param>
        /// <returns></returns>
        [HttpGet("view-payment/{PayId}")]
        public async Task<IActionResult> GetPayment([FromRoute]long PayId)
        {
            var pay = await Paymentservice.GetPayment(PayId);
            if(pay is not null)
            {
                return JsonStatusResponse.Success(message:"success" ,ReturnData:pay );
            }
            return JsonStatusResponse.NotFound("payment not founf");
        }
        #endregion


        #region get all payments
        /// <summary>
        /// Api for get pay list {get request from query}
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("payments-list")]
        public async Task<IActionResult> GetAllPayments([FromQuery] ReqFilterPayDto filter)
        {
            var Pay = await Paymentservice.GetAllPayments(filter);
            if (Pay.Pay.Any())
            {
                return JsonStatusResponse.Success(message: "bia bekhoresh", ReturnData: Pay);
            }
            return JsonStatusResponse.NotFound(message: "nist ke bekhorishi");
        }
        #endregion


        #region Edit pay
        /// <summary>
        /// Api for edit payments details by admin {get request from body}
        /// </summary>
        /// <param name="Paydto"></param>
        /// <returns></returns>
        [HttpPut("edit-payment")]
        public async Task<IActionResult> EditPay([FromBody] EditPayDto Paydto)
        {
            var res = await Paymentservice.EditPay(Paydto);
            switch (res)
            {
                case ResProject.Success:
                    return JsonStatusResponse.Success("The payment has been updated successfully");
                case ResProject.Faild:
                    return JsonStatusResponse.Error("payment update failed. Try again ‌later.");
                case ResProject.NotFound:
                    return JsonStatusResponse.Error("Invalid input");
                default:
                    return JsonStatusResponse.Error("An error has occurred. Try again later.");
            }
        }
        #endregion

    }
}
