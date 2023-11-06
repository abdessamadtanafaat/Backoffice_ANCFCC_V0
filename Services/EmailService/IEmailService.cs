namespace Backoffice_ANCFCC.Services.EmailService
{
    public interface IEmailService
    {
        Task SendEmail(string toEmail, string subject, string htmlMessage);

    }
}
