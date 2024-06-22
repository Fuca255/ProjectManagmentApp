using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace server.Models
{
    [Table("notification_user")]
    [PrimaryKey("UserId","NotificationId")]
    public class NotificationUser
    {
        [Key]
        public int UserId { get; set; }
        [Key]
        public int NotificationId { get; set; }
        public bool MarkAsRead { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("NotificationUsers")]
        public virtual User User { get; set; } = null!;
        [ForeignKey("NotificationId")]
        [InverseProperty("NotificationUsers")]
        public virtual Notification Notification { get; set; } = null!;
    }
}