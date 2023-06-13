using Donations_App.Data;
using Donations_App.Models;
using Donations_App.Models.Payment;
using Donations_App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Text;

namespace Donations_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPaymentService _payment;
        public PaymentController(ApplicationDbContext context , IPaymentService payment)
        {
            
            _payment = payment;
        }
        [Authorize]
        [HttpPost("Check-Credit/{CartId}")]
        public async Task<ActionResult> CheckCredit(int CartId)
        {
            var framesOfPayment = await _payment.CheckCredit(CartId);
            return Ok(framesOfPayment);

        }
        [HttpPost]
        [Route("payment-callback")]
        public async Task<IActionResult> PaymentCallback([FromBody] ResponsePayment data)
        {
            
            var result = await _payment.PaymentCallback(data);  
            
            return Ok(result);
        }

        [Authorize]
        [HttpPost("Check-Credit-Mobile/{CartId}")]
        public async Task<ActionResult> CheckCreditMobile(int CartId)
        {
            var framesOfPayment = await _payment.CheckCredit(CartId);
            return Ok(framesOfPayment);

        }

        [HttpPost]
        [Route("payment-callback-mobile")]
        public async Task<IActionResult> PaymentCallbackMobile([FromBody] ResponsePayment data)
        {

            var result = await _payment.PaymentCallback(data);

            return Ok(result);
        }
        //[HttpGet]
        //[Route("payment-callback/{token}")]
        //public async Task<IActionResult> CallbackResponse(string token)
        //{



        //    return Ok(success);
        //}

    }
}
