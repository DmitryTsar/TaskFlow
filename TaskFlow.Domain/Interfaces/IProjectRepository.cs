using TaskFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskFlow.Domain.Interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllAsync();
        Task<Project?> GetByIdAsync(Guid id);
        Task<Project> AddAsync(Project project);
        Task<Project?> UpdateAsync(Project project);
        Task<bool> DeleteAsync(Guid id);

        // Возвращает задачи проекта без включений проекта
        Task<IEnumerable<TaskItem>> GetTasksAsync(Guid projectId);
        // Возвращает проект с включёнными задачами и их основными навигациями
        Task<Project?> GetProjectWithTasksAsync(Guid projectId);
    }
}
