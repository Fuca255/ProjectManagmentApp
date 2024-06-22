using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.DTOs.Answer
{
    public class CreateAnswerDto
    {
        public string Content { get; set; } = null!;
        public int UserId { get; set; }
        public int CommentId { get; set; }
    }
}