using Donations_App.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DonationsApp.Models
{
    public class VerifyCode
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
