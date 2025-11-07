using MediatR;
using System;

namespace TaskFlow.Application.Commands
{
    public class DeleteAttachmentCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
