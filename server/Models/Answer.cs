using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    [Table("answer")]
    public class Answer
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("content")]
        public string? Content { get; set; }

        [Column("post_time", TypeName = "datetime")]
        public DateTime PostTime { get; set; }

        [Column("time", TypeName = "datetime")]
        public DateTime EditedTime { get; set; }

        [Column("comment_id")]
        public int CommentId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Answers")]
        public virtual User User { get; set; } = null!;
        [ForeignKey("CommentId")]
        [InverseProperty("Answers")]
        public virtual Comment? Comment { get; set; }
    }
}