using ITour.Services.Email;
using ITour.Services.Email.SendGrid;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace WebPWrecover.Services
{
    public class SendGridEmailSender : IEmailSender
    {
        public SendGridEmailSender(IOptionsSnapshot<EmailSenderOptions> namedOptionsAccessor)
        {
            SenderOptions = namedOptionsAccessor.Get("EmailSender");
        }

        public EmailSenderOptions SenderOptions { get; }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(SenderOptions.SendGrid.SendGridKey, email, subject, message );
        }

        public Task Execute(string sendGridKey, string email, string subject, string message)
        {
            SendGridClient sendGridClient = new SendGridClient(sendGridKey);
            SendGridMessage sendGridMessage = new SendGridMessage()
            {
                From = new EmailAddress(SenderOptions.AppEmailAddress, SenderOptions.AppEmailName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            sendGridMessage.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            sendGridMessage.SetClickTracking(false, false);

            return sendGridClient.SendEmailAsync(sendGridMessage);
        }
    }
}