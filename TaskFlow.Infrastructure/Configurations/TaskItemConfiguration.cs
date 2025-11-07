using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Persistence.Configurations
{
    public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            builder.ToTable("Tasks");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(t => t.Description)
                   .HasMaxLength(2000);

            builder.Property(t => t.Status)
                   .IsRequired();

            builder.HasOne(t => t.Project)
                   .WithMany(p => p.Tasks)
                   .HasForeignKey(t => t.ProjectId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.CreatedBy)
                   .WithMany(u => u.CreatedTasks)
                   .HasForeignKey(t => t.CreatedById)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.AssignedTo)
                   .WithMany(u => u.AssignedTasks)
                   .HasForeignKey(t => t.AssignedToId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(t => t.History)
                   .WithOne(h => h.Task)
                   .HasForeignKey(h => h.TaskId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Comments)
                   .WithOne(c => c.Task)
                   .HasForeignKey(c => c.TaskId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Attachments)
                   .WithOne(a => a.Task)
                   .HasForeignKey(a => a.TaskId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
