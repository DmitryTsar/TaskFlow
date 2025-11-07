using TaskStatus = TaskFlow.Domain.Enums.TaskStatus;

namespace TaskFlow.Domain.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ProjectId { get; set; }
        public Guid CreatedById { get; set; }
        public Guid? AssignedToId { get; set; }

        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.New;

        // Таймстемпы
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DoneAt { get; set; }
        public DateTime? DueDate { get; set; }

        // Навигация
        public User CreatedBy { get; set; } = default!;
        public User? AssignedTo { get; set; }
        public Project Project { get; set; } = default!;
        public ICollection<TaskHistory> History { get; set; } = new List<TaskHistory>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    }
}
