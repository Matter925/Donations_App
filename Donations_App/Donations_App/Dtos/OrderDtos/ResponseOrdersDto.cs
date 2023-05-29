using Donations_App.Models;

namespace Donations_App.Dtos.OrderDtos
{
    public class ResponseOrdersDto
    {
        public IEnumerable<Order> Orders { get; set; }
        public int Count { get; set; }
        public double Total { get; set; }   
    }
}
