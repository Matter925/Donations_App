using System.ComponentModel.DataAnnotations;

namespace Donations_App.Models
{
    public class ChangePasswordDto
    {
        
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        

    }
}
