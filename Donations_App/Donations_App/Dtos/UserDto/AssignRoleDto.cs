using System.ComponentModel.DataAnnotations;

namespace Donations_App.Models
{
    public class AssignRoleDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
