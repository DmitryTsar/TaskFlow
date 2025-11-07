using MediatR;
using TaskFlow.Application.DTOs;
using System;
using System.Collections.Generic;

namespace TaskFlow.Application.Commands
{
    public class GetProjectTasksCommand : IRequest<IEnumerable<TaskForProjectDto>>
    {
        public Guid ProjectId { get; set; }
    }
}
