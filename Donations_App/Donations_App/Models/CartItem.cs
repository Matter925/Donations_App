using DonationsApp.Models;

namespace Donations_App.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public double setAmount { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; }
        public int PatientCaseId { get; set; }
        public PatientCase PatientCase { get; set; }
    }
}
