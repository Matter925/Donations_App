namespace Donations_App.Dtos.UserDto
{
    public class GetAllUsers
    {
        public IEnumerable<UserModel> Users { get; set; }
        public int Count { get; set; }
    }
}
