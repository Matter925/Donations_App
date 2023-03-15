using Donations_App.Models;

namespace Donations_App.Dtos.RequestDtos
{
    public class UserRequestsDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string UserId { get; set; }
        public IEnumerable<Request> Requests { get; set; }
        
    }
}
