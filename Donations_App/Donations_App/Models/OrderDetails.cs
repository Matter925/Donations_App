namespace Donations_App.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int PaymentOrderId { get; set; }
        public int OrderId { get; set; }
       public Order order { get; set; }

    }
}
