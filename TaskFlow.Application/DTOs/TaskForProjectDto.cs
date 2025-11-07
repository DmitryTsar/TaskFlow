using System;

namespace TaskFlow.Application.DTOs
{
    public class TaskForProjectDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public Guid AssignedToId { get; set; }
        public string AssignedToName { get; set; } = default!;
        public string Status { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}
