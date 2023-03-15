using Donations_App.Models;

namespace Donations_App.Dtos.CartDtos
{
    public class ItemsCartDto
    {
        public IEnumerable<CartItem> Items { get; set; }
        public int Count { get; set; }
        public double Total { get; set; }   
    }
}
