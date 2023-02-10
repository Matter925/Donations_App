using System.ComponentModel.DataAnnotations;

namespace Donations_App.Dtos.CartDtos
{
    public class CartItemAddDto
    {
        public int PatientCaseId { get; set; }
        [Range(1,10000)]
        public double setAmount { get; set; }
        public int CartId { get; set; }
    }
}
