using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    [Table("tag")]
    public class Tag
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name", TypeName = "varchar(45)")]
        public string Name { get; set; } = null!;

        [Column("category", TypeName = "varchar(45)")]
        public string Category { get; set; } = null!;

        [Column("color", TypeName = "varchar(45)")]
        public string Color { get; set; } = null!;

        [ForeignKey("TagId")]
        [InverseProperty("Tags")]
        public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

        [ForeignKey("TagId")]
        [InverseProperty("Tags")]
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

        [InverseProperty("Tag")]
        public virtual ICollection<TeamTag> TeamTags { get; set; } = new List<TeamTag>();
        }
}