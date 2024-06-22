using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    [Table("state")]
    public class State
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(40)")]
        public string Name { get; set; } = string.Empty;


        //1-to-many
        [InverseProperty("State")]
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

        [InverseProperty("State")]
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    }
}