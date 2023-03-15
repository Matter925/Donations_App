using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Donations_App.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public int OrderId { get; set; }

        [JsonIgnore]
        public Order Order { get; set; }
        public int PatientCaseId { get; set; }
        public PatientCase PatientCase { get; set; }


    }
}
