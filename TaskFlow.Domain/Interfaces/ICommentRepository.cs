using TaskFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskFlow.Domain.Interfaces
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAllAsync();
        Task<Comment?> GetByIdAsync(Guid id);
        Task<Comment> AddAsync(Comment comment);
        Task<Comment?> UpdateAsync(Comment comment);
        Task<bool> DeleteAsync(Guid id);

        Task<IEnumerable<Comment>> GetByTaskIdAsync(Guid taskId);
        Task<IEnumerable<Comment>> GetByUserIdAsync(Guid userId);
    }
}
