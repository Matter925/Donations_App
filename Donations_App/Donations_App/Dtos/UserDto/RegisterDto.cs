using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Donations_App.Models
{
    public class RegisterDto
    {
        [Required , StringLength(100)]
        public string FirstName { get; set; }
        [Required, StringLength(100)]
        public string LastName { get; set; }

        [Required, StringLength(100)]
        public string Username { get; set; }
        [Required, StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required, StringLength(100)]
       
        public string Password { get; set; }
        public string Address { get; set; }
    }
}
