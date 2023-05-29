using Donations_App.Data;
using Donations_App.Repositories.OrderItemsServices;
using Donations_App.Repositories.OrderServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Donations_App.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Get-All-Orders")]
        public async Task<IActionResult> GetOrders()
        {
            
            return Ok(await _orderRepository.GetAllOrders());
        }

        [HttpGet("Get-User-Orders/{UserID}")]
        public async Task<IActionResult> GetOrdersByUserId(string UserID)
        {
            return Ok(await _orderRepository.GetOrderByUserId(UserID));
        }

        [HttpDelete("DeleteOrder/{OrderId}")]
        public async Task<IActionResult> DeleteOrder(int OrderId)
        {
            var result = await _orderRepository.DeleteOrder(OrderId);
            if(result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
    }
}
