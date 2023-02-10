using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Donations_App.Dtos.CategoryDtos
{
    public class CategoryDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(2500)]
        public string Description { get; set; }
        [Required]
        public IFormFile Image { get; set; }
    }
}
