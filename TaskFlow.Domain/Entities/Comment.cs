namespace TaskFlow.Domain.Entities
{
    public class Comment
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Content { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid AuthorId { get; set; }
        public User Author { get; set; } = default!;

        public Guid TaskId { get; set; }
        public TaskItem Task { get; set; } = default!;
    }
}
