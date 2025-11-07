using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;
using TaskFlow.Infrastructure.Persistence;

namespace TaskFlow.Infrastructure.Repositories
{
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly TaskFlowDbContext _context;

        public AttachmentRepository(TaskFlowDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Attachment>> GetAllAsync()
        {
            return await _context.Attachments
                .Include(a => a.Task)
                .ToListAsync();
        }

        public async Task<Attachment?> GetByIdAsync(Guid id)
        {
            return await _context.Attachments
                .Include(a => a.Task)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Attachment> AddAsync(Attachment attachment)
        {
            _context.Attachments.Add(attachment);
            await _context.SaveChangesAsync();
            return attachment;
        }

        public async Task<Attachment?> UpdateAsync(Attachment attachment)
        {
            var existing = await _context.Attachments.FindAsync(attachment.Id);
            if (existing == null) return null;

            _context.Entry(existing).CurrentValues.SetValues(attachment);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var attachment = await _context.Attachments.FindAsync(id);
            if (attachment == null) return false;

            _context.Attachments.Remove(attachment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Attachment>> GetByTaskAsync(Guid taskId)
        {
            return await _context.Attachments
                .Where(a => a.TaskId == taskId)
                .ToListAsync();
        }
    }
}
