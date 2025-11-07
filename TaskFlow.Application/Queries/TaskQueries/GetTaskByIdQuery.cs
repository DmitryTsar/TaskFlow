using MediatR;
using TaskFlow.Application.DTOs;
using System;

namespace TaskFlow.Application.Queries.TaskQueries
{
    public class GetTaskByIdQuery : IRequest<TaskDto>
    {
        public Guid Id { get; set; }
    }
}
