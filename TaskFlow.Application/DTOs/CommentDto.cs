using System;

namespace TaskFlow.Application.DTOs
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = default!;
        public DateTime CreatedAt { get; set; }

        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;

        public Guid TaskId { get; set; }
        public string TaskTitle { get; set; } = string.Empty;
    }
}
