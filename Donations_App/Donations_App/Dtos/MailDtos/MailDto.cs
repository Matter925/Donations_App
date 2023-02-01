using System.ComponentModel.DataAnnotations;

namespace Donations_App.Dtos.MailDtos
{
    public class MailDto
    {
        [Required]
        public string ToEmail { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }
        public IList<IFormFile> Attachments { get; set; }
    }
}
