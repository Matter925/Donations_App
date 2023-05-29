using System.ComponentModel.DataAnnotations;

namespace Donations_App.Dtos.UserDto

{
    public class VerifyCodeDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
