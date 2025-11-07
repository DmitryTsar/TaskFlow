using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;
using TaskFlow.Infrastructure.Persistence;

namespace TaskFlow.Infrastructure.Repositories
{
    public class TaskHistoryRepository : ITaskHistoryRepository
    {
        private readonly TaskFlowDbContext _context;

        public TaskHistoryRepository(TaskFlowDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskHistory>> GetByTaskIdAsync(Guid taskId)
        {
            return await _context.TaskHistories
                .Include(h => h.ChangedBy)
                .Where(h => h.TaskId == taskId)
                .OrderByDescending(h => h.ChangedAt)
                .ToListAsync();
        }

        public async Task AddAsync(TaskHistory history)
        {
            _context.TaskHistories.Add(history);
            await _context.SaveChangesAsync();
        }
    }
}
