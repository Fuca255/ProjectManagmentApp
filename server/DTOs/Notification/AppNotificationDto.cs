using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.DTOs.Users;

namespace server.DTOs.Notification
{
    public class AppNotificationDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime DateSent { get; set; }
        public bool MarkAsRead { get; set; }
    }
}