using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public UserRole Role { get; set; } = UserRole.User;

        // Навигационные свойства
        public ICollection<Project> OwnedProjects { get; set; } = new List<Project>();
        public ICollection<TaskItem> CreatedTasks { get; set; } = new List<TaskItem>(); // добавляем для CreatedById
        public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<TaskHistory> HistoryChanges { get; set; } = new List<TaskHistory>();
    }
}
