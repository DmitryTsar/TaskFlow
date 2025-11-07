using MediatR;
using TaskFlow.Application.DTOs;
using System;
using System.Collections.Generic;

namespace TaskFlow.Application.Queries
{
    public class GetAttachmentsByTaskIdQuery : IRequest<IEnumerable<AttachmentDto>>
    {
        public Guid TaskId { get; set; }
    }
}
