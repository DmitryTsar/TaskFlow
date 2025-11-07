using System;
using System.Threading.Tasks;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Repositories;
using Xunit;

namespace TaskFlow.Tests.RepositoryTests
{
    public class AttachmentRepositoryTests
    {
        [Fact]
        public async Task AddAndGetAttachment_ShouldWork()
        {
            var context = TestDbContextFactory.CreateInMemory();
            var user = new User { UserName = "creator", Email = "c@test.com", PasswordHash = "hash" };
            var project = new Project { Name = "Proj", Owner = user };
            var task = new TaskItem { Title = "Task", Project = project, CreatedBy = user };
            context.Users.Add(user);
            context.Projects.Add(project);
            context.Tasks.Add(task);
            await context.SaveChangesAsync();

            var repo = new AttachmentRepository(context);
            var attachment = new Attachment
            {
                FileName = "file.txt",
                FilePath = "/files/file.txt",
                FileSize = 100,
                TaskId = task.Id
            };
            await repo.AddAsync(attachment);

            var fetched = await repo.GetByTaskAsync(task.Id);
            Assert.Contains(fetched, a => a.Id == attachment.Id);
        }
    }
}
