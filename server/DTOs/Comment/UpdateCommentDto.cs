using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.DTOs.Comment
{
    public class UpdateCommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
    }
}