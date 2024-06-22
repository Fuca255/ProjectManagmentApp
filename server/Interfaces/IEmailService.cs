

namespace server.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailDto request);
        Task SendCustomNotificationAsync(NotificationDto notification);
    }
}
