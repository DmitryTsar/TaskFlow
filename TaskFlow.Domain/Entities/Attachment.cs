namespace TaskFlow.Domain.Entities
{
    public class Attachment
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FileName { get; set; } = default!;
        public string FilePath { get; set; } = default!; // относительный путь или URL
        public long FileSize { get; set; }

        public Guid TaskId { get; set; }
        public TaskItem Task { get; set; } = default!;

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
