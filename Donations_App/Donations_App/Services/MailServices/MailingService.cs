using Donations_App.Dtos.ReturnDto;
using Donations_App.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Donations_App.Services
{
    public class MailingService : IMailingService
    {
        private readonly MailSettings _mailSettings;
        public MailingService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public async Task<GeneralRetDto> SendEmailAsync(string mailTo, string subject, string body, IList<IFormFile> attachments = null)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Email),
                Subject = subject,
            };
            email.To.Add(MailboxAddress.Parse(mailTo));
            var builder = new BodyBuilder();
            if(attachments != null)
            {
                byte[] fileBytes;
                foreach(var file in attachments)
                {
                    if(file.Length > 0)
                    {
                        using var ms = new MemoryStream();
                        file.CopyTo(ms);
                        fileBytes = ms.ToArray();   
                        builder.Attachments.Add(file.FileName , fileBytes , ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName , _mailSettings.Email));
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host , _mailSettings.Port , SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Email, _mailSettings.Password);
            var maiSend = await smtp.SendAsync(email);
            if (maiSend.StartsWith("2"))
            {
                smtp.Disconnect(true);
                return new GeneralRetDto
                {
                    Success = true,
                    Message ="Sucess"
                };
            }
            return new GeneralRetDto
            {
                Success = false,
                Message = "Email Is Not Valid"
            };

        }
    }
}
