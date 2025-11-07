using MediatR;
using TaskFlow.Application.DTOs;
using System;

namespace TaskFlow.Application.Commands
{
    public class UpdateAttachmentCommand : IRequest<AttachmentDto?>
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = default!;
        public string FilePath { get; set; } = default!;
        public long FileSize { get; set; }
    }
}
