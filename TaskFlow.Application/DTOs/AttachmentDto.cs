using System;

namespace TaskFlow.Application.DTOs
{
    public class AttachmentDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = default!;
        public string FilePath { get; set; } = default!;
        public long FileSize { get; set; }
        public DateTime UploadedAt { get; set; }

        public Guid TaskId { get; set; }
        public string TaskTitle { get; set; } = string.Empty;
    }
}
