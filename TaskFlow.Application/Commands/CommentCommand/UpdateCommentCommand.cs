using MediatR;
using TaskFlow.Application.DTOs;
using System;

namespace TaskFlow.Application.Commands
{
    public class UpdateCommentCommand : IRequest<CommentDto?>
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = default!;
    }
}
