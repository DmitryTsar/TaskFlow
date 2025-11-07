using MediatR;
using TaskFlow.Application.DTOs;
using System;
using System.Collections.Generic;

namespace TaskFlow.Application.Queries
{
    public class GetCommentsByTaskIdQuery : IRequest<IEnumerable<CommentDto>>
    {
        public Guid TaskId { get; set; }
    }
}
