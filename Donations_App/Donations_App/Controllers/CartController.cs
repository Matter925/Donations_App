using Donations_App.Dtos.CartDtos;
using Donations_App.Repositories.CartItemServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Donations_App.Controllers
{
    
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartItemRepository _cartItemRepository;
        public CartController(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }
        [HttpGet("GetItems/{CartID}")]
        public async Task<IActionResult> GetItems(int CartID)
        {
            
            var cartItems = await _cartItemRepository.GetItems(CartID);
            if(cartItems == null)
            {
                return NotFound("The Cart Id Not Found !! ");
            }
            return Ok(cartItems);
        }

        [HttpGet("GetItemById/{ItemId}")]
        public async Task<IActionResult> GetItemById(int ItemId)
        {
            var cartItem = await _cartItemRepository.GetItem(ItemId);
            if (cartItem == null)
            {
                return NotFound("The Item Id Not Found !! ");
            }
            return Ok(cartItem);
        }

        [HttpPost("AddItem")]
        public async Task<ActionResult> AddItem([FromBody] CartItemAddDto cartItemAddDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _cartItemRepository.AddItem(cartItemAddDto);

                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            
            return BadRequest(ModelState);
        }

        [HttpPost("UpdateAmount")]
        public async Task<IActionResult> UpdateAmountItem(UpdateAmountDto dto)
        {
            if(ModelState.IsValid)
            {
                var result = await _cartItemRepository.UpdateAmount(dto);
                if(result.Success)
                {
                    return Ok(result);
                }
                return NotFound(result);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("DeleteItem/{ItemId}")]
        public async Task<IActionResult> DeleteItem(int ItemId)
        {
            var result = await _cartItemRepository.DeleteItem(ItemId);
            if(result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpDelete("DeleteAll/{cartId}")]
        public async Task<IActionResult> DeleteAll(int cartId)
        {
            var result = await _cartItemRepository.DeleteAll(cartId);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
    }
}
