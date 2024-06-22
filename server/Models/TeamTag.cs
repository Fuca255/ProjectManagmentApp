using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace server.Models
{
    [PrimaryKey("TeamId","TagId")]
    [Table("team_tag")]
    public class TeamTag
    {
        [Key]
        [Column("team_id")]
        public int TeamId { get; set; }

        [Key]
        [Column("tag_id")]
        public int TagId { get; set; }

        [ForeignKey("TeamId")]
        [InverseProperty("TeamTags")]
        public Team? Team { get; set; }

        [ForeignKey("TagId")]
        [InverseProperty("TeamTags")]
        public Tag? Tag { get; set; }
    }
}