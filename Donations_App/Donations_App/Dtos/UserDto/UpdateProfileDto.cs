using System.ComponentModel.DataAnnotations;

namespace Donations_App.Dtos.UserDto
{
    public class UpdateProfileDto
    {
        
        [Required, StringLength(100)]
        public string FullName { get; set; }

        [Required , EmailAddress]
        public string Email { get; set; }

        public string UserName { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }

        [Required, StringLength(100)]
        public string Address { get; set; }
    }
}
