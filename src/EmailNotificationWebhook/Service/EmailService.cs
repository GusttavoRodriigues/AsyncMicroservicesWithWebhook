using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Shared.DTOs;


namespace EmailNotificationWebhook.Service
{
    public class EmailService : IEmailService
    {
        public string SendEmail(EmailDTO email)
        {
            var _email = new MimeMessage();
            _email.From.Add(MailboxAddress.Parse(""));
            _email.To.Add(MailboxAddress.Parse(""));
            _email.Subject = email.Title;
            _email.Body = 
                    new TextPart(MimeKit.Text.TextFormat.Html)
                    { Text = email.Content };

            using var smtp = new SmtpClient();
            smtp.Connect("", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("", "", CancellationToken.None);
            smtp.Send(_email);
            smtp.Disconnect(true);

            return "Email send";
        }
    }
}
