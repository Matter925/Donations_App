using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Donations_App.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CartId { get; set; }
        public double TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public bool OrderStatus { get; set; }
        public int PaymentOrderId { get; set; }

        public  List<OrderItem> OrderItems { get; set; }
        [ForeignKey("UserId")]
        [JsonIgnore]
        public ApplicationUser User { get; set; }
    }
}
