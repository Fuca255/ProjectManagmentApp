using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTOs.Comment;
using server.Models;

namespace server.Repositories
{
    public class CommentRepositroy : ICommentRepository
    {
        private readonly ProjectManagmentDbContext _context;
        public CommentRepositroy(ProjectManagmentDbContext context)
        {
            _context = context;
        }
        public async Task<Comment> CreateCommentAsync(Comment comment)
        {
            comment.PostTime = DateTime.Now;
            comment.EditedTime = DateTime.Now;
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> DeleteCommentByIdAsync(int comment_id)
        {
            var comment = await GetCommentByIdAsync(comment_id);
            if (comment==null)
                return null;
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<List<Comment>> GetAllCommentsByAssignmentIdAsync(int asignment_id)
        {
            return await _context.Comments.Include(c=>c.User).Include(c=>c.Answers).ThenInclude(a=>a.User).Where(c=>c.AssignmentId==asignment_id).ToListAsync();
        }

        public async Task<Comment?> GetCommentByIdAsync(int comment_id)
        {
            return await _context.Comments.Include(c=>c.User).Include(c=>c.Answers).ThenInclude(a=>a.User).FirstOrDefaultAsync(c=>c.Id==comment_id);
        }

        public async Task<Comment?> UpdateCommentAsync(UpdateCommentDto dto)
        {
            var comment = await GetCommentByIdAsync(dto.Id);
            if(comment==null)
                return null;
            
            comment.Content = dto.Content;
            comment.EditedTime = DateTime.Now;

            await _context.SaveChangesAsync();

            return comment;
        }
    }
}