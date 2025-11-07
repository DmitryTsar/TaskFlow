using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.UserName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasIndex(u => u.Email)
                   .IsUnique();

            builder.Property(u => u.PasswordHash)
                   .IsRequired();

            builder.Property(u => u.Role)
                   .IsRequired();

            builder.HasMany(u => u.OwnedProjects)
                   .WithOne(p => p.Owner)
                   .HasForeignKey(p => p.OwnerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.CreatedTasks)
                   .WithOne(t => t.CreatedBy)
                   .HasForeignKey(t => t.CreatedById)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.AssignedTasks)
                   .WithOne(t => t.AssignedTo)
                   .HasForeignKey(t => t.AssignedToId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(u => u.Comments)
                   .WithOne(c => c.Author)
                   .HasForeignKey(c => c.AuthorId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.HistoryChanges)
                   .WithOne(h => h.ChangedBy)
                   .HasForeignKey(h => h.ChangedById)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
