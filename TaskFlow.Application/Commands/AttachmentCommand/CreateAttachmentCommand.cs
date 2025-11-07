using MediatR;
using TaskFlow.Application.DTOs;
using System;

namespace TaskFlow.Application.Commands
{
    public class CreateAttachmentCommand : IRequest<AttachmentDto>
    {
        public string FileName { get; set; } = default!;
        public string FilePath { get; set; } = default!;
        public long FileSize { get; set; }
        public Guid TaskId { get; set; }
    }
}
