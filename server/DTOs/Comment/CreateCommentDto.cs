using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.DTOs.Comment
{
    public class CreateCommentDto
    {
        public string Content { get; set; } = null!;
        public int UserId { get; set; }
        public int AssignmentId { get; set; }
    }
}