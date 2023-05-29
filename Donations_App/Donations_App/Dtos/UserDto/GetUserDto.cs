namespace Donations_App.Dtos.UserDto
{
    public class GetUserDto
    {
       public bool Success { get; set; }
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        public string Username { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }
        public int CartId { get; set; }
        public List<string> Roles { get; set; }
        public string Token { get; set; }
        public DateTime ExpireOn { get; set; }

        //[JsonIgnore]
        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiration { get; set; }

    }
}
