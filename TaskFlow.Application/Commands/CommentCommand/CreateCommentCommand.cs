using MediatR;
using TaskFlow.Application.DTOs;
using System;

namespace TaskFlow.Application.Commands
{
    public class CreateCommentCommand : IRequest<CommentDto>
    {
        public string Content { get; set; } = default!;
        public Guid AuthorId { get; set; }
        public Guid TaskId { get; set; }
    }
}
