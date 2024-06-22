using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    [Table("team")]
    public class Team
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("createdBy")]
        public int CreatedBy { get; set; }

        [Column("name", TypeName = "varchar(45)")]
        public string Name { get; set; } = null!;

        [Column("type", TypeName = "varchar(45)")]
        public string Type { get; set; } = null!;


        [ForeignKey("CreatedBy")]
        [InverseProperty("CreatedTeams")]
        public virtual User Owner {get; set;} = null!;

        //many-to-many condition
        [InverseProperty("Teams")]
        public virtual ICollection<User> Users {get; set;} = new List<User>();

        [InverseProperty("Team")]
        public virtual ICollection<TeamTag> TeamTags { get; set; } = new List<TeamTag>();

    }
}