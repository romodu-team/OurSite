using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OurSite.Core.DTOs.Payment;
using OurSite.Core.Services.Interfaces.Projecta;
using OurSite.Core.Utilities;
using OurSite.DataLayer.Entities.Payments;
using OurSite.DataLayer.Interfaces;
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
        private IGenericRepository<Payment> PaymentRepositories;
        public PaymentController(IPayment paymentservice , IGenericRepository<Payment> PaymentRepositories)
        {
            this.Paymentservice = paymentservice;
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
                {
                    HttpContext.Response.StatusCode = 200;
                    return JsonStatusResponse.Success(_zarinPalResponseModel, "Payment was successful");
                }
                else
                {
                    HttpContext.Response.StatusCode = 409;
                    return JsonStatusResponse.Error("The transaction has already been verified");
                }
            }
            else
            {
                HttpContext.Response.StatusCode = 500;
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
        [HttpPost("create-payment-by-admin")]
         public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDto paydto)
         {
                var res = await Paymentservice.CreatePayment(paydto);
                switch (res)
                {
                    case ResProject.Success:
                        HttpContext.Response.StatusCode = 201;
                        return JsonStatusResponse.Success(message:"Payment create sucessfully" , ReturnData: paydto);
                    case ResProject.Faild:
                        HttpContext.Response.StatusCode = 500;
                        return JsonStatusResponse.Error("Payment creat Faild");
                    case ResProject.InvalidInput:
                        HttpContext.Response.StatusCode = 204;
                        return JsonStatusResponse.InvalidInput();
                case ResProject.NotFound:
                    HttpContext.Response.StatusCode = 404;
                    return JsonStatusResponse.NotFound("Project to create payment not found");
                    default:
                    HttpContext.Response.StatusCode = 500;
                    return JsonStatusResponse.UnhandledError();

                }
                HttpContext.Response.StatusCode = 401;
                return JsonStatusResponse.Error("U must be login");

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
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success(message:"success" ,ReturnData:pay );
            }
            HttpContext.Response.StatusCode = 404;
            return JsonStatusResponse.NotFound("payment not found");
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
                HttpContext.Response.StatusCode = 200;
                return JsonStatusResponse.Success(message: "bia bekhoresh", ReturnData: Pay);
            }
            HttpContext.Response.StatusCode = 404;
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
                    HttpContext.Response.StatusCode = 200;
                    return JsonStatusResponse.Success(message: "The payment has been updated successfully" , ReturnData: Paydto);
                case ResProject.Faild:
                    HttpContext.Response.StatusCode = 500;
                    return JsonStatusResponse.Error("payment update failed. Try again ‌later.");
                case ResProject.NotFound:
                    HttpContext.Response.StatusCode = 404;
                    return JsonStatusResponse.Error("Invalid input");
                default:
                    HttpContext.Response.StatusCode = 500;
                    return JsonStatusResponse.UnhandledError();
            }
        }
        #endregion

        #region Delete paymet
        [HttpDelete("delete-payment")]
        public async Task<IActionResult> DeletePayment([FromQuery]long PayId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var delete = await Paymentservice.DeletePayment(PayId , true);
                switch (delete)
                {
                    case ResProject.Success:
                        HttpContext.Response.StatusCode = 200;
                        return JsonStatusResponse.Success(message: "Payment delete successfully" , ReturnData: PayId);
                    case ResProject.Faild:
                        HttpContext.Response.StatusCode = 500;
                        return JsonStatusResponse.Error("Payment delete failed.");
                    case ResProject.Error:
                        HttpContext.Response.StatusCode = 404;
                        return JsonStatusResponse.NotFound("payment not found");
                    default:
                        HttpContext.Response.StatusCode = 500;
                        return JsonStatusResponse.UnhandledError();
                }
            }
            HttpContext.Response.StatusCode = 403;
            return JsonStatusResponse.Error("u didnt login. please login first");

        }
        #endregion



        #region view project's payment
        [HttpGet("get-list-payments-of-project")]
        public async Task<IActionResult> GetPayments([FromQuery]long ProjectId)
        {
            var result = await Paymentservice.GetPayments(ProjectId);
            if (result is not null)
            {
                return JsonStatusResponse.Success(ReturnData: result,"sucsess");
            }
            return JsonStatusResponse.NotFound("faild");
        }
        #endregion

    }
}
