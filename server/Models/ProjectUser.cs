using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace server.Models
{
    [PrimaryKey("ProjectId","UserId")]
    [Table("project_user")]
    public class ProjectUser
    {
        [Key]
        [Column("project_id")]
        public int ProjectId { get; set; }
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }
        
        [Column("project_role_id")]
        public int ProjectRoleId { get; set; }
        
        [ForeignKey("UserId")]
        [InverseProperty("ProjectUsers")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("ProjectId")]
        [InverseProperty("ProjectUsers")]
        public virtual Project Project { get; set; } = null!;


        [ForeignKey("ProjectRoleId")]
        [InverseProperty("ProjectUsers")]
        public virtual Role Role { get; set; } = null!;
    }
}