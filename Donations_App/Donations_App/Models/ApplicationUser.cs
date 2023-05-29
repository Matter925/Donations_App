using Donations_App.Models;
using DonationsApp.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Donations_App.Models
{
    public class ApplicationUser :IdentityUser
    {
        [Required ,MaxLength(100)]
        public string FullName { get; set; }
       
        [Required]
        public string Address { get; set; }
        [JsonIgnore]
        public List<RefreshToken>? RefreshTokens { get; set; }
        [JsonIgnore]
        public Cart Cart { get; set; }
        
        public ICollection<Request> Requests { get; set; }
        public ICollection<Order> Orders { get; set; }
       


    }
}
