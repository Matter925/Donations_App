using Donations_App.Dtos.RequestDtos;

namespace Donations_App.Dtos.UserDto
{
    public class UsersDetails
    {
       public IEnumerable<UserRequestsDto>  Users { get; set; }
        public int CountUsers { get; set; }
    }
}
