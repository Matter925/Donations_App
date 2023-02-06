using Donations_App.Data;
using Donations_App.Dtos.CartDtos;
using Donations_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Donations_App.Repositories.CartItemServices
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly ApplicationDbContext _context;
        public CartItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<CartItem> AddItem(CartItemAddDto cartItemAdd)
        {
            
            var cartItem = new CartItem
            {
                PatientCaseId = cartItemAdd.PatientCaseId,
                CartId = cartItemAdd.CartId,
                setAmount = cartItemAdd.setAmount
            };
            await _context.AddAsync(cartItem);
            await _context.SaveChangesAsync();
            
            return await _context.CartItems.Include(p=>p.PatientCase).SingleOrDefaultAsync(c=> c.CartId== cartItemAdd.CartId);

        }
    }
}
