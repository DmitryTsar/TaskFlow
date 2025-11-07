using MediatR;
using TaskFlow.Application.DTOs;
using System;

namespace TaskFlow.Application.Queries
{
    public class GetCommentByIdQuery : IRequest<CommentDto?>
    {
        public Guid Id { get; set; }
    }
}
