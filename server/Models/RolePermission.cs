using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class RolePermission
    {
        [Key] 
        public int Id { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}
