using Donations_App.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Donations_App.Models
{
    public class ApplicationUser :IdentityUser
    {
        [Required ,MaxLength(100)]
        public string FirstName { get; set; }
        [Required, MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; }

        public List<RefreshToken>? RefreshTokens { get; set; }



    }
}
