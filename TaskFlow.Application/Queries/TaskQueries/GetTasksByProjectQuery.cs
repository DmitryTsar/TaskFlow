using MediatR;
using TaskFlow.Application.DTOs;
using System;
using System.Collections.Generic;

namespace TaskFlow.Application.Queries.TaskQueries
{
    public class GetTasksByProjectQuery : IRequest<IEnumerable<TaskDto>>
    {
        public Guid ProjectId { get; set; }
    }
}
