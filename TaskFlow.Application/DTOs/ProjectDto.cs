using System;
using System.Collections.Generic;

namespace TaskFlow.Application.DTOs
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid OwnerId { get; set; }
        public string OwnerName { get; set; } = default!;
    }
}
