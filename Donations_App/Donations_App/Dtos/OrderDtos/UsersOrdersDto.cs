using Donations_App.Models;

namespace Donations_App.Dtos.OrderDtos
{
    public class UsersOrdersDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string UserId { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
