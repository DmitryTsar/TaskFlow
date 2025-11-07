namespace TaskFlow.Domain.ValueObjects
{
    public class AuditInfo
    {
        public Guid CreatedById { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Guid? UpdatedById { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public AuditInfo(Guid createdById)
        {
            CreatedById = createdById;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(Guid userId)
        {
            UpdatedById = userId;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}

