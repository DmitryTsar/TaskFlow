using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;
using TaskFlow.Domain.Interfaces;
using TaskFlow.Infrastructure.Persistence;
using TaskStatus = TaskFlow.Domain.Enums.TaskStatus;

namespace TaskFlow.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskFlowDbContext _context;

        public TaskRepository(TaskFlowDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            return await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .ToListAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task<TaskItem> AddAsync(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TaskItem?> UpdateAsync(TaskItem task)
        {
            var existing = await _context.Tasks.FindAsync(task.Id);
            if (existing == null) return null;

            _context.Entry(existing).CurrentValues.SetValues(task);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TaskItem>> GetByProjectAsync(Guid projectId)
        {
            return await _context.Tasks
                .Where(t => t.ProjectId == projectId)
                .Include(t => t.AssignedTo)
                .Include(t => t.Comments)
                .Include(t => t.Attachments)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetByUserAsync(Guid userId)
        {
            return await _context.Tasks
                .Where(t => t.CreatedById == userId || t.AssignedToId == userId)
                .Include(t => t.Project)
                .Include(t => t.AssignedTo)
                .Include(t => t.CreatedBy)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetByStatusAsync(Guid projectId, TaskStatus status)
        {
            return await _context.Tasks
                .Where(t => t.ProjectId == projectId && t.Status == status)
                .Include(t => t.AssignedTo)
                .Include(t => t.Project)
                .ToListAsync();
        }

        public async Task<TaskItem?> GetFullTaskAsync(Guid id)
        {
            return await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .Include(t => t.Comments).ThenInclude(c => c.Author)
                .Include(t => t.Attachments)
                .Include(t => t.History).ThenInclude(h => h.ChangedBy)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
