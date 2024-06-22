using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.DTOs.Answer;
using server.Mappers;
using server.Models;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _user_repo;

        public AnswerController(IUserRepository user_repo,IAnswerRepository answerRepository,ICommentRepository commentRepository)
        {
            _answerRepository = answerRepository;
            _commentRepository = commentRepository;
            _user_repo = user_repo;
        }

        [HttpPost("CreateAnswer"), Authorize(Roles = "Project Manager,Project Worker,Member")]
        public async Task<ActionResult<Answer>> CreateAnswer(CreateAnswerDto dto)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(dto.CommentId);
            if (comment == null)
            {
                return NotFound("Comment not found."); // Return NotFound with a message
            }
            var user = await _user_repo.GetUserByIdAsync(dto.UserId);
            if (user == null)
                return NotFound("User not found");

            var answer = dto.fromCreateDtoToAnswer();

            answer = await _answerRepository.CreateAnswerAsync(answer);

            return Ok(answer.toAnswerDto(user.toUserDto()));
        }

        [HttpDelete("{id}"), Authorize(Roles = "Project Manager,Project Worker,Member")]
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            var answer = await _answerRepository.DeleteAnswerByIdAsync(id);

            if (answer == null)
            {
                return NotFound();
            }

            return NoContent();
        }
        [Authorize]
        [HttpGet("getAllAnswersByComment/{commentId}")]
        public async Task<ActionResult<IEnumerable<AnswerDto>>> GetAllAnswersByCommentId(int commentId)
        {
            var answers = await _answerRepository.GetAllAnswersByCommentIdAsync(commentId);

            // if (!answers.Any())
            // {
            //     return NotFound("No Answers");
            // }

            var answerDtos = answers.Select(a=>a.toAnswerDto(a.User.toUserDto()));

            return Ok(answerDtos);
        }
        [HttpPut("updateAnswer"), Authorize(Roles = "Project Manager,Project Worker,Member")]
        public async Task<IActionResult> UpdateAnswerById(UpdateAnswerDto dto)
        {
            var answer = await _answerRepository.UpdateAnswerAsync(dto);
            if(answer ==null)
                return NotFound("Answer not found !!!");
            
            return Ok(answer.toAnswerDto(answer.User.toUserDto()));

        }

    }
}
