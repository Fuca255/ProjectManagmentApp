using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    [Table("project")]
    public class Project
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("title", TypeName = "varchar(45)")]
        public string Title { get; set; } = null!;

        [Column("start", TypeName = "datetime")]
        public DateTime Start { get; set; }

        [Column("end", TypeName = "datetime")]
        public DateTime End { get; set; }
        [Column("owner_id")]
        public int OwnerId { get; set; }

        [Column("description")]
        public string Description { get; set; } = null!;

        [Column("state")]
        public int StateId { get; set; }

        [Column("last_state_changed", TypeName ="datetime")]
        public DateTime LastStateChangedTime { get; set; }

        [Column("budget", TypeName = "double")]
        public double Budget { get; set; }

        [Column("spent", TypeName = "double")]
        public double Spent { get; set; }

        [Column("type", TypeName = "varchar(45)")]
        public string Type { get; set; } = null!;

        [Column("percentage", TypeName = "float")]
        public double Percentage { get; set; }

        [Column("priority_id")]
        public int PriorityId { get; set; }

        [ForeignKey("PriorityId")]
        [InverseProperty("Projects")]
        public virtual Priority Priority { get; set; } = null!;
        

        [ForeignKey("ProjectId")]
        [InverseProperty("Projects")]
        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

        //1-to-many
        [ForeignKey("StateId")]
        [InverseProperty("Projects")]
        public virtual State State { get; set; } = null!;


        [InverseProperty("Project")]
        public virtual ICollection<TaskGroup> TaskGroups { get; set; } = new List<TaskGroup>();

        [InverseProperty("Project")]
        public virtual ICollection<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();

        
        [InverseProperty("Project")]
        public virtual ICollection<UserProjectPermission> ProjectPermissions { get; set; } = new List<UserProjectPermission>();

        [ForeignKey("OwnerId")]
        [InverseProperty("Projects")]
        public virtual User Owner { get; set; } = null!;
    }
}