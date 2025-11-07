using MediatR;
using System;

namespace TaskFlow.Application.Commands
{
    public class DeleteCommentCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
