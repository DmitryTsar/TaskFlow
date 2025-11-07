using TaskFlow.Domain.Entities;

namespace TaskFlow.Domain.Interfaces
{
    public interface ITaskHistoryRepository
    {
        Task<IEnumerable<TaskHistory>> GetByTaskIdAsync(Guid taskId);
        Task AddAsync(TaskHistory history);
    }
}
