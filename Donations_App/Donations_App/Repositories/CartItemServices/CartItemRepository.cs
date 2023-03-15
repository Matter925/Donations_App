using Donations_App.Data;
using Donations_App.Dtos.CartDtos;
using Donations_App.Dtos.ReturnDto;
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
        public async Task<GeneralRetDto> AddItem(CartItemAddDto dto)
        {
            var IsExist = await _context.CartItems.AnyAsync(c=> c.CartId==dto.CartId && c.PatientCaseId == dto.PatientCaseId);
            if (IsExist)
            {
                return new GeneralRetDto
                {
                    Success = false,
                    Message ="The Item is exist"
                };
            }
            
            var cartItem = new CartItem
            {
                PatientCaseId = dto.PatientCaseId,
                CartId = dto.CartId,
                setAmount = dto.setAmount
            };
            await _context.AddAsync(cartItem);
            await _context.SaveChangesAsync();

            return new GeneralRetDto
            {
                Success = true,
                Message = "Successfully Added"
            }; 

        }

        public async Task<GeneralRetDto> DeleteAll(int cartId)
        {
            var item = await _context.Carts.FindAsync(cartId);
            if (item == null)
            {
                return new GeneralRetDto
                {
                    Success = false,
                    Message = "Cart Id is not found !!"
                };
            }
            var items = await _context.CartItems.Where(e => e.CartId == cartId).ToListAsync();
            if(items.Any())
            {
                foreach (var ex in items)
                {
                    _context.CartItems.Remove(ex);
                }
                await _context.SaveChangesAsync();
                return new GeneralRetDto
                {
                    Success = true,
                    Message = "Successfully Deleted Items"
                };
            }
            return new GeneralRetDto
            {
                Success = true,
                Message = "No Items Founded In Cart"
            };

        }

        public async Task<GeneralRetDto> DeleteItem(int ItemId)
        {
            var item = await _context.CartItems.FindAsync(ItemId);
            if (item == null)
            {
                return new GeneralRetDto
                {
                    Success = false,
                    Message = "Item Id is not found !!"
                };
            }
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            return new GeneralRetDto
            {
                Success= true,
                Message ="Successfully Deleted"
            };

        }

        public async Task<CartItem> GetItem(int ItemId)
        {
            var IsExist = await _context.CartItems.FindAsync(ItemId);
            if (IsExist == null)
            {
                return null;
            }
            var Item = await _context.CartItems.Include(c => c.PatientCase).Include(c => c.PatientCase.Category).SingleOrDefaultAsync(r => r.Id == ItemId);
            return Item;

        }

        public async Task<ItemsCartDto> GetItems(int CartId)
        {
            double total = 0;
            var IsExist = await _context.Carts.FindAsync(CartId);
            if(IsExist==null)
            {
                return null;
            }
            var Items = await _context.CartItems.Include(c => c.PatientCase).Include(c=> c.PatientCase.Category).Where(o => o.CartId == CartId).ToListAsync();
            foreach (var Item in Items)
            {
                total += Item.setAmount;
            };
            return new ItemsCartDto
            {
                Items = Items,
                Count = Items.Count(),
                Total = total
            };
        }

        public async Task<GeneralRetDto> UpdateAmount(UpdateAmountDto dto)
        {
            var item = await _context.CartItems.FindAsync(dto.ItemID);
            if(item==null)
            {
                return new GeneralRetDto
                {
                    Success=false,
                    Message ="Item Id is not found !!"
                };
            }
            item.setAmount = dto.newAmount;
            _context.CartItems.Update(item);
            await _context.SaveChangesAsync();
            return new GeneralRetDto { 
                Success = true ,
                Message ="Successfully Updated"
                
            };
        }
    }
}
