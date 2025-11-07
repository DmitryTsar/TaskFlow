using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Persistence.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Content)
                   .IsRequired()
                   .HasMaxLength(2000);

            builder.HasOne(c => c.Author)
                   .WithMany(u => u.Comments)
                   .HasForeignKey(c => c.AuthorId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Task)
                   .WithMany(t => t.Comments)
                   .HasForeignKey(c => c.TaskId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
