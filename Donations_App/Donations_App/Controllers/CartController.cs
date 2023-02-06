using Donations_App.Dtos.CartDtos;
using Donations_App.Repositories.CartItemServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Donations_App.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartItemRepository _cartItemRepository;
        public CartController(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }
        [HttpPost("AddItem")]
        public async Task<ActionResult> AddItem([FromBody] CartItemAddDto cartItemAddDto)
        {
            var newCartItem = await _cartItemRepository.AddItem(cartItemAddDto);

            if (newCartItem == null)
            {
                return NoContent();
            }

            return Ok(newCartItem);

            



        }
    }
}
