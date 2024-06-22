using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.DTOs.Notification;
using server.Mappers;
using server.Models;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly INotificationRepository _notificationRepo;
        public NotificationController(IUserRepository userRepository,INotificationRepository notificationRepo)
        {
            _notificationRepo = notificationRepo;
            _userRepository = userRepository;
        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDto dto)
        {
            List<User> users = new List<User>();
            foreach (var userId in dto.UserIds)
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                if(user==null)
                    return NotFound("User not found");
                users.Add(user);
            }
            var notification =dto.fromCreateDtoToNotificationDto();
            notification =await _notificationRepo.CreateNotification(notification,users);

            return Ok(notification.toNotificationDto(true));
        }

        [Authorize]
        [HttpGet("getByUser/{user_id}")]
        public async Task<IActionResult> GetAllNotifications(int user_id)
        {
            var user = await _userRepository.GetUserByIdAsync(user_id);
            if(user==null)
                return NotFound("User not found");
            var notifications = await _notificationRepo.GetAllNotificationsByUserAsync(user_id);
            return Ok(notifications.Select(x=>x.Notification.toNotificationDto(x.MarkAsRead)));
        }
        [Authorize]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody]UpdateNotificationDto dto)
        {
            var notification = await _notificationRepo.UpdateNotificationState(dto.UserId,dto.NotificationId);
            if(notification==null)
                return NotFound("Notification or user not found");
            
            return Ok(notification.Notification.toNotificationDto(true));
        }
    }
}