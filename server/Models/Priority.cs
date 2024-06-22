using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    [Table("priority")]
    public class Priority
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("level")]
        public int Level { get; set; }

        [Column("description")]
        public string Description { get; set; } = null!;

        [InverseProperty("Priority")]
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

        [InverseProperty("Priority")]
        public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    }
}