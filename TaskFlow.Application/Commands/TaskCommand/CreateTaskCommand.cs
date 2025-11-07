using MediatR;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Enums;
using System;
using TaskStatus = TaskFlow.Domain.Enums.TaskStatus;

namespace TaskFlow.Application.Commands.TaskCommand
{
    public class CreateTaskCommand : IRequest<TaskDto>
    {
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public Guid ProjectId { get; set; }
        public Guid CreatedById { get; set; }
        public Guid? AssignedToId { get; set; }
        public TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
    }
}
