using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime DateSent { get; set; }

        [InverseProperty("Notification")]
        public virtual ICollection<NotificationUser> NotificationUsers { get; set; } = new List<NotificationUser>();
    }
}