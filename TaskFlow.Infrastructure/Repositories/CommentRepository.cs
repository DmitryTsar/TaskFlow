using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;
using TaskFlow.Infrastructure.Persistence;

namespace TaskFlow.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly TaskFlowDbContext _context;

        public CommentRepository(TaskFlowDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _context.Comments
                .Include(c => c.Author)
                .Include(c => c.Task)
                .ThenInclude(t => t.Project)
                .ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(Guid id)
        {
            return await _context.Comments
                .Include(c => c.Author)
                .Include(c => c.Task)
                    .ThenInclude(t => t.Project)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Comment> AddAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> UpdateAsync(Comment comment)
        {
            var existing = await _context.Comments.FindAsync(comment.Id);
            if (existing == null) return null;

            _context.Entry(existing).CurrentValues.SetValues(comment);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return false;

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Comment>> GetByTaskIdAsync(Guid taskId)
        {
            return await _context.Comments
                .Where(c => c.TaskId == taskId)
                .Include(c => c.Author)
                .Include(c => c.Task)
                .ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Comments
                .Where(c => c.AuthorId == userId)
                .Include(c => c.Task)
                    .ThenInclude(t => t.Project)
                .Include(c => c.Author)
                .ToListAsync();
        }
    }
}
