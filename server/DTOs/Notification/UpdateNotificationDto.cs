using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.DTOs.Notification
{
    public class UpdateNotificationDto
    {
        public int UserId { get; set; }
        public int NotificationId { get; set; }
    }
}