using System.ComponentModel.DataAnnotations;

namespace Donations_App.Dtos.RequestDtos
{
    public class RequestDto
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public IFormFile ID_Photo { get; set; }
        [Required]
        public IFormFile Medical_Report { get; set; }
        [Required]
        public string Description_Request { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
