using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class UserProjectRoles
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }

        [ForeignKey("ProjectId")]
        public int ProjectId { get; set; }

        [ForeignKey("RoleId")]
        public int RoleId { get; set; }

        // Navigation properties
        public virtual User? User { get; set; }
        public virtual Project? Project { get; set; }
        public virtual Role? Role { get; set; }
    }
}
