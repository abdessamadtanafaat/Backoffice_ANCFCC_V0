using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace Backoffice_ANCFCC.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration )
        {
            _configuration = configuration; 
        }
        public async Task SendEmail(string toEmail, string subject, string htmlMessage)
        {

            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_configuration.GetSection("EmailUsername").Value));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = new TextPart(TextFormat.Html) { Text = htmlMessage } ;
            using var smtp = new SmtpClient();
            {
                smtp.Connect(_configuration.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_configuration.GetSection("EmailUsername").Value, _configuration.GetSection("EmailPassword").Value);
                smtp.Send(message);
                smtp.Disconnect(true);
            }

        }
    }
}
