using TaskFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskFlow.Domain.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAllAsync();
        Task<TaskItem?> GetByIdAsync(Guid id);
        Task<TaskItem> AddAsync(TaskItem task);
        Task<TaskItem?> UpdateAsync(TaskItem task);
        Task<bool> DeleteAsync(Guid id);

        Task<IEnumerable<TaskItem>> GetByProjectAsync(Guid projectId);
        Task<IEnumerable<TaskItem>> GetByUserAsync(Guid userId);
        Task<IEnumerable<TaskItem>> GetByStatusAsync(Guid projectId, Enums.TaskStatus status);

        Task<TaskItem?> GetFullTaskAsync(Guid id); // Include: Project, CreatedBy, AssignedTo, Comments, Attachments, History
    }
}
