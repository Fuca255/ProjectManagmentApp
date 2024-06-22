using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using server.Email_Body;

namespace server.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //Slanje emaila
        public async Task SendEmailAsync(EmailDto request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("EmailSettings:From").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };
            using var smtp = new SmtpClient();
            smtp.Connect(_configuration.GetSection("EmailSettings:SmtpServer").Value, 465, true);
            smtp.Authenticate(_configuration.GetSection("EmailSettings:Username").Value, _configuration.GetSection("EmailSettings:Password").Value);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        private string GenerateHtmlBody(string notificationType, string parameter1)
        {
            switch (notificationType)
            {
                case "ChangeOnTask":
                    return ChangeOnTask.GetHtmlBody(parameter1);
                case "ChangeOnProject":
                    return ChangeOnProject.GetHtmlBody(parameter1);
                case "AddedToProject":
                    return AddedToProject.GetHtmlBody(parameter1);
                case "AddedToTask":
                    return AddedToTask.GetHtmlBody(parameter1);
                default:
                    return "";
            }
        }

        public async Task SendCustomNotificationAsync(NotificationDto notification)
        {
            string htmlBody = GenerateHtmlBody(notification.NotificationType, notification.TaskOrProjectName);

            using var smtp = new SmtpClient();
            smtp.Connect(_configuration.GetSection("EmailSettings:SmtpServer").Value, 465, true);
            smtp.Authenticate(_configuration.GetSection("EmailSettings:Username").Value, _configuration.GetSection("EmailSettings:Password").Value);

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("EmailSettings:From").Value));
            email.To.Add(MailboxAddress.Parse(notification.To));
            email.Subject = notification.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = htmlBody };

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
