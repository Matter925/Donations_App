using System.ComponentModel.DataAnnotations;

namespace Donations_App.Dtos.UserDto
{
    public class CreatePasswordDto
    {
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required]
        public string RestToken { get; set; }

    }
}
