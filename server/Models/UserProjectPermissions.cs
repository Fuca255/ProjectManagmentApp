using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class UserProjectPermission
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("ProjectId")]
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        // Additional properties
        [ForeignKey("PermissionId")]
        public int PermissionId { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    }
}
