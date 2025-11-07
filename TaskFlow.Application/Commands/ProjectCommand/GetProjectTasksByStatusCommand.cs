using MediatR;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using TaskStatus = TaskFlow.Domain.Enums.TaskStatus;

namespace TaskFlow.Application.Commands
{
    public class GetProjectTasksByStatusCommand : IRequest<IEnumerable<TaskForProjectDto>>
    {
        public Guid ProjectId { get; set; }
        public TaskStatus Status { get; set; }
    }
}
