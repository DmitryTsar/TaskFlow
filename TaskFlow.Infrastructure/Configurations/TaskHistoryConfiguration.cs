using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Persistence.Configurations
{
    public class TaskHistoryConfiguration : IEntityTypeConfiguration<TaskHistory>
    {
        public void Configure(EntityTypeBuilder<TaskHistory> builder)
        {
            builder.ToTable("TaskHistories");

            builder.HasKey(h => h.Id);

            builder.Property(h => h.PropertyName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(h => h.OldValue)
                   .HasMaxLength(500);

            builder.Property(h => h.NewValue)
                   .HasMaxLength(500);

            builder.HasOne(h => h.Task)
                   .WithMany(t => t.History)
                   .HasForeignKey(h => h.TaskId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(h => h.ChangedBy)
                   .WithMany(u => u.HistoryChanges)
                   .HasForeignKey(h => h.ChangedById)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
