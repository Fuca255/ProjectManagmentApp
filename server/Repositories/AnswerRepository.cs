using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTOs.Answer;
using server.Models;

namespace server.Repositories
{
    public class AnswerRepository:IAnswerRepository
    {
        private readonly ProjectManagmentDbContext _context;

        public AnswerRepository(ProjectManagmentDbContext context)
        {
            _context = context;
        }

        public async Task<Answer> CreateAnswerAsync(Answer answer)
        {
            await _context.Answers.AddAsync(answer);
            await _context.SaveChangesAsync();
            return answer;
        }

        public async Task<Answer?> DeleteAnswerByIdAsync(int answerId)
        {
            var answer = await GetAnswerByIdAsync(answerId);
            if(answer == null)
            {
                return null;
            }

            _context.Answers.Remove(answer);
            await _context.SaveChangesAsync();
            return answer;
        }

        public async Task<List<Answer>> GetAllAnswersByCommentIdAsync(int commentId)
        {
            return await _context.Answers.Include(a=>a.User).Where(a => a.CommentId == commentId).ToListAsync();
        }

        public async Task<Answer?> GetAnswerByIdAsync(int answerId)
        {
            return await _context.Answers.Include(a=>a.User).FirstOrDefaultAsync(a=>a.Id== answerId);
        }

        public async Task<Answer?> UpdateAnswerAsync(UpdateAnswerDto dto)
        {
            var answer = await GetAnswerByIdAsync(dto.Id);
            if(answer == null)
            {
                return null;
            }

            answer.Content = dto.Content;
            answer.EditedTime=DateTime.Now;
            return answer;
        }
    }
}
