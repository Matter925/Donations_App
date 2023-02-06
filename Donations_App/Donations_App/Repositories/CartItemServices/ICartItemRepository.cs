using Donations_App.Dtos.CartDtos;
using Donations_App.Models;

namespace Donations_App.Repositories.CartItemServices
{
    public interface ICartItemRepository
    {
        Task<CartItem> AddItem(CartItemAddDto cartItemAdd);
        //Task<CartItem> UpdateQty(CartItemQtyUpdateDto cartItemQtyUpdateDto);
        //Task<CartItem> DeleteItem(int cartItemId);
        //Task<IEnumerable<CartItem>> DeleteAll(int cartId);
        //Task<CartItem> GetItem(int cartItemId);
        //Task<IEnumerable<CartItem>> GetItems(string userId);
    }
}
