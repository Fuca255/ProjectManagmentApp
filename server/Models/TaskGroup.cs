using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace server.Models
{
    [Table("assignment_group")]
    public class TaskGroup
    {
        [Key]
        public int Id { get; set; }
        [Column("title")]
        public string Title { get; set; } = null!;

        [Column("project_id")]
        public int ProjectId { get; set; }
        [Column("parent_task_group_id")]
        public int? ParentTaskGroupId { get; set; } = null;
        [ForeignKey("ProjectId")]
        [InverseProperty("TaskGroups")]

        public virtual Project Project { get; set; } = null!;

        [ForeignKey("ParentTaskGroupId")]
        [InverseProperty("InverseParentNavigation")]
        public virtual TaskGroup? ParentNavigation { get; set; }

        [InverseProperty("ParentNavigation")]
        public virtual ICollection<TaskGroup> InverseParentNavigation { get; set; } = new List<TaskGroup>();

        [InverseProperty("TaskGroup")]
        public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    }
}