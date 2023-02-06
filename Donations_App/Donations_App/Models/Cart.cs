using Donations_App.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace DonationsApp.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public ICollection<CartItem> CartItems { get; set; }

    }
}
