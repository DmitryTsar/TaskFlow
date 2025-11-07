using MediatR;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Enums;
using System;
using System.Collections.Generic;

namespace TaskFlow.Application.Queries.TaskQueries
{
    public class GetTasksByStatusQuery : IRequest<IEnumerable<TaskDto>>
    {
        public Guid ProjectId { get; set; }
        public Domain.Enums.TaskStatus Status { get; set; }
    }
}
