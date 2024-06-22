using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;

namespace server.Interfaces
{
    public interface INotificationRepository
    {
        Task<Notification> CreateNotification(Notification notification, List<User> users);
        Task<List<NotificationUser>> GetAllNotificationsByUserAsync(int user_id);
        Task<Notification?> GetNotificationById(int notification_id);
        Task<NotificationUser?> UpdateNotificationState(int userId, int notification_id);
    
    }   
}