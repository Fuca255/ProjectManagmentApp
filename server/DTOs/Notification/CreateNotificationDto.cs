using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.DTOs.Notification
{
    public class CreateNotificationDto
    {
        public List<int> UserIds { get; set; } = new List<int>();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}