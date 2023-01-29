using System.Text.Json.Serialization;

namespace Donations_App.Models
{
    public class AuthModel
    {
        public string Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        
        public List<string> Roles { get; set; }

        //public int CartId { get; set; }
        public string Token { get; set; }

        public DateTime ExpireOn { get; set; }

        //[JsonIgnore]
        public string? RefreshToken { get; set; }


        public DateTime RefreshTokenExpiration { get; set; }

    }
}
