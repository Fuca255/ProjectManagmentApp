using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;

namespace server.Repositories
{
    public class NotificationRepository : INotificationRepository
    {

        private readonly ProjectManagmentDbContext _context;
        
        public NotificationRepository(ProjectManagmentDbContext context)
        {
            _context = context;
        }

        public async Task<Notification> CreateNotification(Notification notification, List<User> users)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();

            var notUser = new List<NotificationUser>();
            foreach (var user in users)
            {
                notUser.Add(new NotificationUser{
                    User = user,
                    Notification = notification,
                    MarkAsRead = false
                });
            }

            await _context.NotificationUsers.AddRangeAsync(notUser);
            await _context.SaveChangesAsync();

            return notification;
        }

        public async Task<List<NotificationUser>> GetAllNotificationsByUserAsync(int user_id)
        {
            var notifications = _context.NotificationUsers
            .Include(n=>n.Notification)
            .Where(nu=>nu.UserId == user_id && !nu.MarkAsRead);

            return await notifications.ToListAsync();
        }

        public async Task<Notification?> GetNotificationById(int notification_id)
        {
            return await _context.Notifications.Include(n=>n.NotificationUsers).FirstOrDefaultAsync(notification => notification.Id==notification_id);
        }

        public async Task<NotificationUser?> UpdateNotificationState(int user_id, int notification_id)
        {
            var notification = await _context.NotificationUsers.Include(nu=>nu.Notification).FirstOrDefaultAsync(nu=>nu.UserId == user_id && nu.NotificationId==notification_id);
            if(notification!=null)
                notification.MarkAsRead = true;
            else
                return null;
            await _context.SaveChangesAsync();

            return notification;
        }
    }
}