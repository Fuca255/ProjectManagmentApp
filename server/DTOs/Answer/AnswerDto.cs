using server.DTOs.Users;

namespace server.DTOs.Answer
{
    public class AnswerDto
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public DateTime PostTime { get; set; }
        public DateTime EditedTime { get; set; }

        public UserDto User{ get; set; } = null!;
        public int CommentId { get; set; }
    }
}
