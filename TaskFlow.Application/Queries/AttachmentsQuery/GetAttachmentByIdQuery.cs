using MediatR;
using TaskFlow.Application.DTOs;
using System;

namespace TaskFlow.Application.Queries
{
    public class GetAttachmentByIdQuery : IRequest<AttachmentDto?>
    {
        public Guid Id { get; set; }
    }
}
