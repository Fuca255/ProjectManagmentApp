using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }

        [Column("name", TypeName = "varchar(45)")]
        public string Name { get; set; } // Name of the permission, e.g., "read", "write", "create_project", etc.

        [Column("description")]
        public string Description { get; set; } // Description of the permission

        public virtual ICollection<RolePermission> RolePermissions { get; set; }

    }
}
