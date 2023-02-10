using Donations_App.Dtos.CartDtos;
using Donations_App.Dtos.ReturnDto;
using Donations_App.Models;

namespace Donations_App.Repositories.CartItemServices
{
    public interface ICartItemRepository
    {
        Task<IEnumerable<CartItem>> GetItems(int CartId);
        Task<GeneralRetDto> AddItem(CartItemAddDto cartItemAdd);
        Task<GeneralRetDto> UpdateAmount(UpdateAmountDto dto);
        Task<GeneralRetDto> DeleteItem(int ItemId);
        Task<GeneralRetDto> DeleteAll(int cartId);
        Task<CartItem> GetItem(int ItemId);

    }
}
