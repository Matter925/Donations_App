using Donations_App.Repositories.OrderItemsServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Donations_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemsRepository _orderItemsRepository;
        public OrderItemsController(IOrderItemsRepository orderItemsRepository)
        {
            _orderItemsRepository = orderItemsRepository;
        }
        //[HttpGet("Get-Order-Items/{OrderId}")]
        //public async Task<IActionResult> GetOrderItems(int OrderId)
        //{
        //    var result =await _orderItemsRepository.GetItemsByOrderID(OrderId);
        //    if(result == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(result);
        //}
    }
}
