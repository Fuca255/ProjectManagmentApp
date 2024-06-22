using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    [Table("comment")]
    public class Comment
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("content")]
        public string Content { get; set; } = null!;

        [Column("time", TypeName = "datetime")]
        public DateTime EditedTime { get; set; }
        
        [Column("post_time", TypeName ="datetime")]
        public DateTime PostTime { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("assignment_id")]
        public int AssignmentId { get; set; }

        [InverseProperty("Comment")]
        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

        [ForeignKey("AssignmentId")]
        [InverseProperty("Comments")]
        public virtual Assignment Assignment { get; set; } = null!;

        [ForeignKey("UserId")]
        [InverseProperty("Comments")]
        public virtual User User { get; set; } = null!;
    }
}