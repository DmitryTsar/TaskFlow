using TaskFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskFlow.Domain.Interfaces
{
    public interface IAttachmentRepository
    {
        Task<IEnumerable<Attachment>> GetAllAsync();
        Task<Attachment?> GetByIdAsync(Guid id);
        Task<Attachment> AddAsync(Attachment attachment);
        Task<Attachment?> UpdateAsync(Attachment attachment);
        Task<bool> DeleteAsync(Guid id);

        Task<IEnumerable<Attachment>> GetByTaskAsync(Guid taskId);
    }
}
