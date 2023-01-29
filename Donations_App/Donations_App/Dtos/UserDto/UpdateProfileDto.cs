using System.ComponentModel.DataAnnotations;

namespace Donations_App.Dtos.UserDto
{
    public class UpdateProfileDto
    {
        
        [Required, StringLength(100)]
        public string FirstName { get; set; }
        [Required, StringLength(100)]
        public string LastName { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }

        [Required, StringLength(100)]
        public string Address { get; set; }
    }
}
