using TaskFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskFlow.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(Guid id);
        Task<User> AddAsync(User user);
        Task<User?> UpdateAsync(User user);
        Task<bool> DeleteAsync(Guid id);

        Task<IEnumerable<Project>> GetUserProjectsAsync(Guid userId);
        Task<IEnumerable<TaskItem>> GetCreatedTasksAsync(Guid userId);
        Task<IEnumerable<TaskItem>> GetAssignedTasksAsync(Guid userId);

        Task<User?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<User?> GetByUserNameAsync(string userName);
    }
}
