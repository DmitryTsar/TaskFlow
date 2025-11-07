using System;
using System.Collections.Generic;

namespace TaskFlow.Application.DTOs
{
    public class ProjectWithTasksDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid OwnerId { get; set; }
        public string OwnerName { get; set; } = default!;
        public IEnumerable<TaskFlow.Domain.Entities.TaskItem> Tasks { get; set; } = new List<TaskFlow.Domain.Entities.TaskItem>();
    }
}
