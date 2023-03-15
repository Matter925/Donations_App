using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Donations_App.Models
{
    public class Request
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string ID_Photo { get; set; }
        public string Medical_Report { get; set; }
        public string Description_Request { get; set; }
        public string RequestStatus { get; set; }
       
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [JsonIgnore]
        public ApplicationUser User { get; set; }

    }
}
