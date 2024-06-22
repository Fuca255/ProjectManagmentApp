using Microsoft.EntityFrameworkCore.Update.Internal;
using MimeKit.Tnef;
using server.DTOs.Answer;
using server.Models;

namespace server.Interfaces
{
    public interface IAnswerRepository
    {
        Task<Answer> CreateAnswerAsync(Answer answer);
        Task<Answer?> DeleteAnswerByIdAsync(int answerId);
        Task<List<Answer>> GetAllAnswersByCommentIdAsync(int commentId);
        Task<Answer?> GetAnswerByIdAsync(int answerId);
        Task<Answer?> UpdateAnswerAsync(UpdateAnswerDto dto);
    }
}
