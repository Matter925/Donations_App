using Donations_App.Dtos;
using Donations_App.Dtos.OrderDtos;
using Donations_App.Dtos.ReturnDto;
using Donations_App.Models;

namespace Donations_App.Repositories.OrderServices
{
    public interface IOrderRepository
    {
        Task<IEnumerable<UsersOrdersDto>> GetAllOrders();
        Task<ResponseOrdersDto> GetOrderByUserId( string UserId);
        Task<Order> GetByPaymentId(int PaymentOrderId);
        Task<GeneralRetDto> CreateOrder(OrderDto Dto);
        Task<GeneralRetDto> UpdateOrderStatus(int PaymentOrderId);
        Task<GeneralRetDto> DeleteOrder(int OrderId);
    }
}
