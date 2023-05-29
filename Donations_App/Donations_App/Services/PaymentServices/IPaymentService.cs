using Donations_App.Dtos.ReturnDto;
using Donations_App.Models.Payment;

namespace Donations_App.Services
{
    public interface IPaymentService
    {
        public  Task<IFramesOfPayment> CheckCredit(int CartId);
        
        public Task<GeneralRetDto> PaymentCallback(ResponsePayment data);
    }
}
