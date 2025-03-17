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
            _email.From.Add(MailboxAddress.Parse("gusttavosilva038@gmail.com"));
            _email.To.Add(MailboxAddress.Parse("gusttavosilva038@gmail.com"));
            _email.Subject = email.Title;
            _email.Body = 
                    new TextPart(MimeKit.Text.TextFormat.Html)
                    { Text = email.Content };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("gusttavosilva038@gmail.com", "ryeq xqaq bayk cxyb", CancellationToken.None);
            smtp.Send(_email);
            smtp.Disconnect(true);

            return "Email send";
        }
    }
}
