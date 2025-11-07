using MediatR;
using TaskFlow.Application.Commands;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Queries;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Handlers
{
    public class AttachmentHandlers :
        IRequestHandler<CreateAttachmentCommand, AttachmentDto>,
        IRequestHandler<UpdateAttachmentCommand, AttachmentDto?>,
        IRequestHandler<DeleteAttachmentCommand, bool>,
        IRequestHandler<GetAllAttachmentsQuery, IEnumerable<AttachmentDto>>,
        IRequestHandler<GetAttachmentByIdQuery, AttachmentDto?>,
        IRequestHandler<GetAttachmentsByTaskIdQuery, IEnumerable<AttachmentDto>>
    {
        private readonly IAttachmentRepository _repository;
        private readonly ITaskRepository _taskRepository;

        public AttachmentHandlers(IAttachmentRepository repository, ITaskRepository taskRepository)
        {
            _repository = repository;
            _taskRepository = taskRepository;
        }

        public async Task<AttachmentDto> Handle(CreateAttachmentCommand request, CancellationToken cancellationToken)
        {
            var attachment = new Attachment
            {
                FileName = request.FileName,
                FilePath = request.FilePath,
                FileSize = request.FileSize,
                TaskId = request.TaskId
            };

            var created = await _repository.AddAsync(attachment);
            var task = await _taskRepository.GetByIdAsync(created.TaskId);

            return new AttachmentDto
            {
                Id = created.Id,
                FileName = created.FileName,
                FilePath = created.FilePath,
                FileSize = created.FileSize,
                UploadedAt = created.UploadedAt,
                TaskId = created.TaskId,
                TaskTitle = task?.Title ?? ""
            };
        }

        public async Task<AttachmentDto?> Handle(UpdateAttachmentCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByIdAsync(request.Id);
            if (existing == null) return null;

            existing.FileName = request.FileName;
            existing.FilePath = request.FilePath;
            existing.FileSize = request.FileSize;

            var updated = await _repository.UpdateAsync(existing);
            if (updated == null) return null;

            var task = await _taskRepository.GetByIdAsync(updated.TaskId);

            return new AttachmentDto
            {
                Id = updated.Id,
                FileName = updated.FileName,
                FilePath = updated.FilePath,
                FileSize = updated.FileSize,
                UploadedAt = updated.UploadedAt,
                TaskId = updated.TaskId,
                TaskTitle = task?.Title ?? ""
            };
        }

        public async Task<bool> Handle(DeleteAttachmentCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteAsync(request.Id);
        }

        public async Task<IEnumerable<AttachmentDto>> Handle(GetAllAttachmentsQuery request, CancellationToken cancellationToken)
        {
            var attachments = await _repository.GetAllAsync();
            return attachments.Select(a => new AttachmentDto
            {
                Id = a.Id,
                FileName = a.FileName,
                FilePath = a.FilePath,
                FileSize = a.FileSize,
                UploadedAt = a.UploadedAt,
                TaskId = a.TaskId,
                TaskTitle = a.Task?.Title ?? ""
            });
        }

        public async Task<AttachmentDto?> Handle(GetAttachmentByIdQuery request, CancellationToken cancellationToken)
        {
            var a = await _repository.GetByIdAsync(request.Id);
            if (a == null) return null;

            return new AttachmentDto
            {
                Id = a.Id,
                FileName = a.FileName,
                FilePath = a.FilePath,
                FileSize = a.FileSize,
                UploadedAt = a.UploadedAt,
                TaskId = a.TaskId,
                TaskTitle = a.Task?.Title ?? ""
            };
        }

        public async Task<IEnumerable<AttachmentDto>> Handle(GetAttachmentsByTaskIdQuery request, CancellationToken cancellationToken)
        {
            var attachments = await _repository.GetByTaskAsync(request.TaskId);
            return attachments.Select(a => new AttachmentDto
            {
                Id = a.Id,
                FileName = a.FileName,
                FilePath = a.FilePath,
                FileSize = a.FileSize,
                UploadedAt = a.UploadedAt,
                TaskId = a.TaskId,
                TaskTitle = a.Task?.Title ?? ""
            });
        }
    }
}
