using System.ComponentModel.DataAnnotations;

namespace Donations_App.Dtos.CartDtos
{
    public class UpdateAmountDto
    {
        public int ItemID { get; set; }

        [Range(1, 10000)]
        public double newAmount { get; set; }
    }
}
