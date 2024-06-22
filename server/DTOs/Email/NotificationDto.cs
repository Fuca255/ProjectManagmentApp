namespace server.DTOs.Email
{
    public class NotificationDto
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string NotificationType { get; set; } = string.Empty;
        public string TaskOrProjectName { get; set; } = string.Empty;
    }
}
