using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.DTOs.Answer;
using server.DTOs.Users;

namespace server.DTOs.Comment
{
    public class CommentDto
    {
        
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public DateTime EditedTime { get; set; }
        public DateTime PostTime { get; set; }
        public int UserId { get; set; }
        public int AssignmentId { get; set; }
        public List<AnswerDto> Answers { get; set; } = new List<AnswerDto>();
        public UserDto User { get; set; } = null!;
    }
}