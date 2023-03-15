namespace Donations_App.Dtos.OrderDtos
{
    public class OrderItemDto
    {
        public double Amount { get; set; }
        public int OrderId { get; set; }
        public int PatientCaseId { get; set; }
    }
}
