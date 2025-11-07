namespace TaskFlow.Domain.Entities
{
    public class TaskHistory
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid TaskId { get; set; }
        public TaskItem Task { get; set; } = default!;

        public string PropertyName { get; set; } = default!; // Например: "Status", "Assignee"
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }

        public Guid ChangedById { get; set; }
        public User ChangedBy { get; set; } = default!;

        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    }
}
