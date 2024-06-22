using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    [Table("assignment")]
    public class Assignment
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("title", TypeName ="varchar(45)")]
        public string Title { get; set; } = null!;
        [Column("owner")]
        public int Owner { get; set; }

        public bool IsClosed { get; set; }
        [Column("description")]
        public string Description { get; set; } = null!;

        [Column("start", TypeName ="datetime")]
        public DateTime Start { get; set; }
        [Column("lastTimeChanged", TypeName ="datetime")]
        public DateTime LastTimeChanged { get; set; }

        [Column("end", TypeName ="datetime")]
        public DateTime End { get; set; }

        [Column("state_id")]
        public int StateId { get; set; }

        [Column("percentage", TypeName ="float")]
        public float Percentage { get; set; }

        [Column("task_group_id")]
        public int TaskGroupId { get; set; }

        [Column("priority_id")]
        public int PriorityId { get; set; }

        [Column("type")]
        public string Type { get; set; } = null!;

        [InverseProperty("Assignment")]
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

        [InverseProperty("Assignments")]
        public virtual TaskGroup TaskGroup { get; set; } = null!;
        [ForeignKey("PriorityId")]
        [InverseProperty("Assignments")]
        public virtual Priority Priority { get; set; } = null!;


        [ForeignKey("AssignmentId")]
        [InverseProperty("Assignments")]
        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

        [ForeignKey("AssignmentId")]
        [InverseProperty("AssignmentsNavigation")]
        public virtual ICollection<User> Users { get; set; } = new List<User>();
        

        [ForeignKey("StateId")]
        [InverseProperty("Assignments")]
        public virtual State State { get; set; } = null!; 

        [ForeignKey("Owner")]
        [InverseProperty("OwnedAssignments")]
        public virtual User User { get; set; } = null!;

        //dependency
        [ForeignKey("DependentOnAssignmentId")]
        [InverseProperty("DependentOnAssignments")]
        public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

        [ForeignKey("AssignmentId")]
        [InverseProperty("Assignments")]
        public virtual ICollection<Assignment> DependentOnAssignments { get; set; } = new List<Assignment>();
    }
}