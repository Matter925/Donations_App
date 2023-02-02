namespace Donations_App.Dtos.UserDto
{
    public class GetUserDto
    {
        public bool isNotNull  { get; set; }
       
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        public string Username { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        
    }
}
