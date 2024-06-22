using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    [Table("role")]
    public class Role
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        public int Authority { get; set; }

        [Column("name", TypeName = "varchar(45)")]
        public string Name { get; set; } = null!;

        [InverseProperty("Role")]
        public virtual ICollection<User> Users { get; set; } = new List<User>();

        [InverseProperty("Role")]
        public virtual ICollection<ProjectUser> ProjectUsers {get; set;} = new List<ProjectUser>();
        public virtual ICollection<UserProjectRoles> UserProjectRoles { get; set; } = new List<UserProjectRoles>();
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}