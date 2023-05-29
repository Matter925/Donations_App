using System.ComponentModel.DataAnnotations;

namespace Donations_App.Dtos.UserDto
{
    public class GetUserData
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
