namespace TaskFlow.Application.DTOs
{
    public class TaskHistoryDto
    {
        public string PropertyName { get; set; } = default!;
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public string ChangedByName { get; set; } = default!;
        public DateTime ChangedAt { get; set; }
    }
}
