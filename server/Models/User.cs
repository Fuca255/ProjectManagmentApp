using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name",TypeName = "varchar(45)")]
        public string Name { get; set; } = string.Empty;

        [Column("lastname", TypeName = "varchar(45)")]
        public string Lastname { get; set; } = string.Empty;

        [Column("username", TypeName ="varchar(45)")]
        public string Username { get; set; } = string.Empty;

        [Column("password", TypeName = "varchar(45)")]
        public string Password { get; set; } = string.Empty;

        [Column("email", TypeName = "varchar(45)")]
        public string Email { get; set; }= string.Empty;

        [Column("job_title", TypeName = "varchar(45)")]
        public string JobTitle { get; set; } =string.Empty;

        [Column("organisation", TypeName = "varchar(45)")]
        public string Organisation { get; set; } = string.Empty;

        [Column("department", TypeName = "varchar(45)")]
        public string Department { get; set; } = string.Empty;

        public bool IsActivated { get; set; }

        [Column("password_reset_token", TypeName = "varchar(100)")]
        public string? PasswordResetToken { get; set; }

        [Column("password_reset_token_expiry", TypeName ="datetime")]
        public DateTime? PasswordResetTokenExpiry { get; set; }

        [Column("profile_picture", TypeName = "BLOB")]
        public byte[]? ProfilePicture { get; set; }

        [Column("picture_type", TypeName = "varchar(45)")]
        public string PictureType { get; set; } = string.Empty;

        [Column("role_id")]
        public int RoleId { get; set; }
        
        [InverseProperty("User")]
        public virtual List<Answer> Answers { get; set; } = new List<Answer>();

        [InverseProperty("User")]
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

        [ForeignKey("RoleId")]
        [InverseProperty("Users")]
        public virtual Role Role { get; set; } = null!;

        [ForeignKey("UserId")]
        [InverseProperty("Users")]
        public virtual ICollection<Assignment> AssignmentsNavigation { get; set; } = new List<Assignment>();

        [InverseProperty("User")]
        public virtual ICollection<Assignment> OwnedAssignments { get; set; } = new List<Assignment>();

        [InverseProperty("Owner")]
        public virtual ICollection<Team> CreatedTeams { get; set; } = new List<Team>();

        //many-to-many-condition
        [InverseProperty("Users")]
        public virtual ICollection<Team> Teams {get; set;} = new List<Team>();

        [InverseProperty("User")]
        public virtual ICollection<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();

        public virtual ICollection<UserProjectRoles> UserProjectRoles { get; set; } = new List<UserProjectRoles>();

        public virtual ICollection<UserProjectPermission> ProjectPermissions { get; set; } = new List<UserProjectPermission>();

        [InverseProperty("User")]
        public virtual ICollection<NotificationUser> NotificationUsers { get; set; } = new List<NotificationUser>();

        [InverseProperty("Owner")]
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    }
}
