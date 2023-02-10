using Donations_App.Dtos.ReturnDto;

namespace Donations_App.Services
{
    public interface IMailingService
    {
        Task<GeneralRetDto> SendEmailAsync (string mailTo , string subject ,string body , IList<IFormFile>attachments = null);
    }
}
