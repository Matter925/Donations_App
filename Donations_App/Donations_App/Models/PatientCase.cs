using System.ComponentModel.DataAnnotations;

namespace Donations_App.Models
{
    public class PatientCase
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public double Amount { get; set; }
        public double AmountPaid { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
