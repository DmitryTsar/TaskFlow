using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Persistence.Configurations
{
    public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
    {
        public void Configure(EntityTypeBuilder<Attachment> builder)
        {
            builder.ToTable("Attachments");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.FileName)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(a => a.FilePath)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.HasOne(a => a.Task)
                   .WithMany(t => t.Attachments)
                   .HasForeignKey(a => a.TaskId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
