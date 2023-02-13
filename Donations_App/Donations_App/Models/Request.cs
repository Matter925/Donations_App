using System.ComponentModel.DataAnnotations.Schema;

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
        public bool Accepted { get; set; }
        public bool Rejected { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

    }
}
