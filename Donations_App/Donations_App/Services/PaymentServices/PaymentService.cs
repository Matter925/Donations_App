using Donations_App.Data;
using Donations_App.Dtos;
using Donations_App.Dtos.ReturnDto;
using Donations_App.Models;
using Donations_App.Models.Payment;
using Donations_App.Repositories.CartItemServices;
using Donations_App.Repositories.OrderServices;
using Donations_App.Repositories.PatientCaseServices;
using Donations_App.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Donations_App.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly PaymentSettings _paymentSettings;
        private readonly ApplicationDbContext _context;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IPatientCaseRepository _patientCaseRepository;
        private readonly IMailingService _mailingService;
       
        public PaymentService(IOptions<PaymentSettings> paymentSettings, ApplicationDbContext context , ICartItemRepository cartItemRepository , IOrderRepository orderRepository, IPatientCaseRepository patientCaseRepository , IMailingService mailingService) 
        {
            _paymentSettings = paymentSettings.Value;
            _context  = context;
            _cartItemRepository = cartItemRepository;
            _orderRepository = orderRepository;
            _patientCaseRepository = patientCaseRepository;
            _mailingService = mailingService;
        }

        //*****-----------PaymentCallback------------------------------------------

        public async Task<GeneralRetDto> PaymentCallback(ResponsePayment data)
        {
            var PaymentOrderId = data.obj.order.id;
            var transactionId = data.obj.id;
            var amount = data.obj.amount_cents;
            var success = data.obj.success;
            if (success)
            {
                // Payment is successful, update order status, send confirmation email, etc.
                var orderInfo = await _orderRepository.GetByPaymentId(PaymentOrderId);
                var order = await _orderRepository.UpdateOrderStatus(PaymentOrderId);
                if(order.Success)
                {
                    //update paidAmount of PatientCase----------------------------------------------------------
                    var increament = await _patientCaseRepository.IncreamentAmountPaid(orderInfo.CartId);
                    if(increament.Success)
                    {
                        //*********** Send Mail To User **********************************
                        string subject = "تأكيد تبرعك للحالات المرضي";
                        string body = $"  تم التبرع بنجاح بمبلغ : {orderInfo.TotalAmount}" +
                            $", شكرا لتبرعك لمعالجة المرضي والامراض المزمنة " +
                            $" رزقك الله الصحة والعافية .";
                        var user =await _context.Carts.Include(c=> c.User).SingleOrDefaultAsync(d=> d.Id == orderInfo.CartId);
                       var sendMail= await _mailingService.SendEmailAsync(user.User.Email, subject, body);
                        await _cartItemRepository.DeleteAll(user.Id);
                        return new GeneralRetDto
                        {
                            Message = "Succssefully",
                            Success = true,

                        };

                    }
                    return new GeneralRetDto
                    {
                        Message = "Faild in update paidAmount",
                        Success = false,
                    };

                }
                return new GeneralRetDto
                {
                    Message = "Faild in update order status",
                    Success = false,

                };


            }

            // Payment is unsuccessful, update order status, send failure notification email, etc.

            return new GeneralRetDto
            {
                Message = "payment faild",
                Success = false,

            };
        }


        //********************************************************************************************************************
        public async Task<IFramesOfPayment> CheckCredit(int CartId)
        {
            var Api_key = _paymentSettings.Api_key;
            var URLToken = _paymentSettings.URLToken;
            var URLOrder = _paymentSettings.URLOrder;
            var URLPayKey = _paymentSettings.URLPayKey;
            int Integration_Id = _paymentSettings.Integration_Id;

            var cartItem = await _cartItemRepository.GetItems(CartId);
            if (! cartItem.Items.Any())
            {
                return new IFramesOfPayment
                {
                    Message = "The Cart is empty !!",
                    Success = false,
                };
            };
            var cart = await _context.Carts.FindAsync(CartId);

            double totalAmount = cartItem.Total * 100;

            //----------------------------------------------------------------------------------------------

            var AuthToken = await GetAuthToken(Api_key , URLToken);

            var getOrderId = await SecodStep(AuthToken.token , totalAmount , URLOrder);

            //--------------addNewOrder--------------------------------------------------------------------------

            var order = new OrderDto
            {
                UserId = cart.UserId ,
                TotalAmount = cartItem.Total,
                OrderDate = DateTime.Now,
                OrderStatus = false ,
                CartId = CartId,
                PaymentOrderId = getOrderId.id,
            };
            var addorder = await _orderRepository.CreateOrder(order);
            if(! addorder.Success)
            {
                return new IFramesOfPayment
                {
                    Message = addorder.Message,
                    Success = false,
                };
            };

            //------------------------------------------------------------------------------------------------------
            var thirdToken = await ThirdStep(AuthToken.token, getOrderId.id , totalAmount , URLPayKey , Integration_Id);
            var PaymentToken = thirdToken.token;
            var cardPayment = await CardPayment(PaymentToken);
            return new IFramesOfPayment
            {
                Message = "Successfully",
                Success = true,
                iFramMasterCard = cardPayment.iFramMasterCard,
                iFramVisa = cardPayment.iFramVisa,
            };
        }

        //-------------------------------------------------------------------------------------------------------------

        

        

        //-----------------------------------------------------------------------------------------------------------------------------------
        private async Task<authToken> GetAuthToken(string Api_key , string URL)
        {
            
            
            var json = new
            {
                api_key = Api_key
            };
            var jsonString = JsonSerializer.Serialize(json);
            HttpClient httpClient = new HttpClient();
            StringContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(URL, httpContent);

            var Result = await response.Content.ReadAsStringAsync();
            authToken ContentDto = JsonSerializer.Deserialize<authToken>(Result);
            
            return new authToken
            {
                token = ContentDto.token,
            };


        }
        private async Task<OrderResponse> SecodStep(string token , double totalAmount ,string URLOrder)
        {
            
            var json = new 
            {
                auth_token = token,
                delivery_needed = false,
                amount_cents = totalAmount,
                currency = "EGP",
                items = new List<object> { },

            };
            var jsonString = JsonSerializer.Serialize(json);
            HttpClient httpClient = new HttpClient();
            StringContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(URLOrder, httpContent);

            var Result = await response.Content.ReadAsStringAsync();

            OrderResponse ContentDto = JsonSerializer.Deserialize<OrderResponse>(Result);

            return new OrderResponse
            {
                id = ContentDto.id,

            };

        }
        private async Task<authToken> ThirdStep(string token, decimal id , double totalAmount ,string URLPayKey , int Integration_Id) 
        {
            
            var json = new
            {
                auth_token = token,
                amount_cents = totalAmount,
                expiration = 3600,
                order_id = id,
                billing_data = new
                {
                    apartment = "803",
                    email = "claudette09@exa.com",
                    floor = "42",
                    first_name = "Clifford",
                    street = "Ethan Land",
                    building = "8028",
                    phone_number = "+86(8)9135210487",
                    shipping_method = "PKG",
                    postal_code = "01898",
                    city = "Jaskolskiburgh",
                    country = "CR",
                    last_name = "Nicolas",
                    state = "Utah"
                },
                currency = "EGP",
                integration_id = Integration_Id

            };
            var jsonString = JsonSerializer.Serialize(json);
            HttpClient httpClient = new HttpClient();
            StringContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(URLPayKey, httpContent);

            var Result = await response.Content.ReadAsStringAsync();

            authToken ContentDto = JsonSerializer.Deserialize<authToken>(Result);

            return new authToken
            {
                token = ContentDto.token,

            };
        }

        private async Task<IFramesOfPayment> CardPayment(string TokenPayment)
        {
            var iFramMasterCard = $"https://accept.paymob.com/api/acceptance/iframes/722271?payment_token={TokenPayment} ";


            var iFramVisa = $"https://accept.paymob.com/api/acceptance/iframes/722272?payment_token={TokenPayment} ";

            return new IFramesOfPayment
            {
                iFramMasterCard = iFramMasterCard,
                iFramVisa = iFramVisa,
            };


        }

        
    }
}
