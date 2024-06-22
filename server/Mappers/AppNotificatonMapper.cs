using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.DTOs.Notification;
using server.Models;

namespace server.Mappers
{
    public static class AppNotificatonMapper
    {
        public static Notification fromCreateDtoToNotificationDto(this CreateNotificationDto dto)
        {
            return new Notification{
                DateSent = DateTime.Now,
                Title = dto.Title,
                Description = dto.Description,

            };

        }

        public static AppNotificationDto toNotificationDto(this Notification notification, bool read)
        {
            return new AppNotificationDto{
                Id = notification.Id,
                Title = notification.Title,
                Description = notification.Description,
                DateSent = notification.DateSent,
                MarkAsRead = read
            };
        }
    }
}