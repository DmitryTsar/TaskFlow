using System;
using TaskFlow.Domain.Enums;
using TaskStatus = TaskFlow.Domain.Enums.TaskStatus;

namespace TaskFlow.Application.DTOs
{
    public class TaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; } = default!;
        public Guid CreatedById { get; set; }
        public string CreatedByName { get; set; } = default!;
        public Guid? AssignedToId { get; set; }
        public string AssignedToName { get; set; } = default!;
        public TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
