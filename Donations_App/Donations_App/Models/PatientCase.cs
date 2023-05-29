using System.ComponentModel.DataAnnotations;

namespace Donations_App.Models
{
    public class PatientCase
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public double AmountPaid { get; set; }
        public string ImageName { get; set; }
        public bool IsComplete { get; set; }
        public string Address { get; set; }
        public DateTime PatientCaseDate { get; set; }
        public int DonationCount { get; set; }
        public int LimitTime { get; set; }
        public int Rate { get; set; }
        public string UserId { get; set; }  
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        
    }
}
