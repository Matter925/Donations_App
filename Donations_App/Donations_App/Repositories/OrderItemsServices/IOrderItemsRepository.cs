using Donations_App.Dtos.OrderDtos;
using Donations_App.Dtos.ReturnDto;
using Donations_App.Models;

namespace Donations_App.Repositories.OrderItemsServices
{
    public interface IOrderItemsRepository
    {
        Task<GeneralRetDto> CreateOrderItems(int PaymentOrderId);
        Task<IEnumerable<OrderItem>> GetItemsByOrderID(int OrderId);
    }
}
