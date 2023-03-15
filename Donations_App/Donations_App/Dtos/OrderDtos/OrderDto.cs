namespace Donations_App.Dtos
{
    public class OrderDto
    {
        public string UserId { get; set; }
        public int CartId { get; set; }
        public double TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public bool OrderStatus  { get; set; }
        public int PaymentOrderId { get; set; }
    }
}
