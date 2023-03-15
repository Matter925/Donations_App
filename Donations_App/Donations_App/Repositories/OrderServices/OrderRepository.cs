using Donations_App.Data;
using Donations_App.Dtos;
using Donations_App.Dtos.OrderDtos;
using Donations_App.Dtos.ReturnDto;
using Donations_App.Models;
using Donations_App.Repositories.OrderItemsServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Donations_App.Repositories.OrderServices
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderItemsRepository _orderItemsRepository;
        public OrderRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context , IOrderItemsRepository orderItemsRepository)
        {
            _context = context;
            _orderItemsRepository = orderItemsRepository;
            _userManager = userManager;
        }
        public async Task<GeneralRetDto> CreateOrder(OrderDto Dto)
        {
            var cartItem = await _context.CartItems.Where(c=>c.CartId == Dto.CartId).ToListAsync();
            if(cartItem == null)
            {
                return new GeneralRetDto
                {
                    Message ="Cart is empty",
                    Success = false,
                };
            }
            var order = new Order
            {
                UserId = Dto.UserId,
                TotalAmount = Dto.TotalAmount,
                OrderDate = Dto.OrderDate,
                CartId = Dto.CartId,
                PaymentOrderId = Dto.PaymentOrderId,

            };
            await _context.AddAsync(order);
             _context.SaveChanges();

           await _orderItemsRepository.CreateOrderItems(Dto.PaymentOrderId);

            return new GeneralRetDto
            {
                Message ="success",
                Success = true,
            };
        }

        public async Task<IEnumerable<UsersOrdersDto>> GetAllOrders()
        {
            //return await _context.Orders.Include(c => c.OrderItems).ThenInclude(d => d.PatientCase).ToListAsync();
            var userOrders = await _userManager.Users.Include(r => r.Orders).ThenInclude(d => d.OrderItems).ThenInclude(p=>p.PatientCase).Where(d => d.Orders.Any()).Select(u => new UsersOrdersDto
            {
                FullName = u.FullName,
                Email = u.Email,
                Phone = u.PhoneNumber,
                UserId = u.Id,
                Orders = u.Orders.ToList(),

            }).ToListAsync();

            return userOrders;
        }

        public Task<Order> GetByPaymentId(int PaymentOrderId)
        {
            return _context.Orders.SingleOrDefaultAsync(o=> o.PaymentOrderId == PaymentOrderId);
        }

        public async Task<ResponseOrdersDto> GetOrderByUserId(string UserId)
        {
            var orders = await _context.Orders.Include(c=>c.OrderItems).ThenInclude(d=>d.PatientCase).Where(o => o.UserId == UserId && o.OrderStatus==true).ToListAsync();
            return new ResponseOrdersDto
            {
                Orders = orders,
                Count = orders.Count()
            };
        }

        public async Task<GeneralRetDto> UpdateOrderStatus(int PaymentOrderId)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(c=> c.PaymentOrderId == PaymentOrderId);
            if(order == null)
            {
                return new GeneralRetDto
                {
                    Success = false,
                    Message = "",
                };
            }
            order.OrderStatus = true;
            _context.Orders.Update(order);
            _context.SaveChanges(true);
            return new GeneralRetDto
            {
                Success = true,
                Message = "Successfully Updated",
            };
        }
        public async Task<GeneralRetDto> DeleteOrder(int OrderId)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(c => c.Id == OrderId);
            if (order == null)
            {
                return new GeneralRetDto
                {
                    Success = false,
                    Message = "Order Not Found",
                };
            }
            
            _context.Orders.Remove(order);
            _context.SaveChanges(true);
            return new GeneralRetDto
            {
                Success = true,
                Message = "Successfully Deleted",
            };
        }
    }
}
