using Donations_App.Data;
using Donations_App.Dtos.OrderDtos;
using Donations_App.Dtos.ReturnDto;
using Donations_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Donations_App.Repositories.OrderItemsServices
{
    public class OrderItemsRepository : IOrderItemsRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderItemsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<GeneralRetDto> CreateOrderItems(int PaymentOrderId)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(c=>c.PaymentOrderId == PaymentOrderId);
            if (order == null)
            {
                return new GeneralRetDto
                {
                    Message = "Order not found ",
                    Success = false,
                };
            }
            var CartItems = await _context.CartItems.Where(c => c.CartId == order.CartId).ToListAsync();
            
            if(CartItems == null)
            {
                return new GeneralRetDto
                {
                    Message ="Cart Is Empty !!",
                    Success =false,
                };
            }
            foreach (var dto in CartItems)
            {
                var item = new OrderItem
                {
                    Amount =dto.setAmount,
                    OrderId =order.Id,
                    PatientCaseId =dto.PatientCaseId,
                };
                await _context.AddAsync(item);
                _context.SaveChanges();
            }

            return new GeneralRetDto
            {
                Message = "success",
                Success = true,
            };
        }

        public async Task<IEnumerable<OrderItem>> GetItemsByOrderID(int OrderId)
        {
            var IsExist = await _context.Orders.FindAsync(OrderId);
            if(IsExist == null)
            {
                return null;
            }
            var orderItems = await _context.OrderItems.Include(c=>c.PatientCase).Where(c=>c.OrderId == OrderId).ToListAsync();
            return orderItems;

        }
    }
}
