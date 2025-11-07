using System;
using System.Threading.Tasks;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Repositories;
using Xunit;

namespace TaskFlow.Tests.RepositoryTests
{
    public class CommentRepositoryTests
    {
        [Fact]
        public async Task AddAndGetComment_ShouldWork()
        {
            var context = TestDbContextFactory.CreateInMemory();
            var user = new User { UserName = "author", Email = "author@test.com", PasswordHash = "hash" };
            var project = new Project { Name = "Proj", Owner = user };
            var task = new TaskItem { Title = "Task", Project = project, CreatedBy = user };
            context.Users.Add(user);
            context.Projects.Add(project);
            context.Tasks.Add(task);
            await context.SaveChangesAsync();

            var repo = new CommentRepository(context);
            var comment = new Comment
            {
                Content = "Hello",
                AuthorId = user.Id,
                TaskId = task.Id
            };
            await repo.AddAsync(comment);

            var fetched = await repo.GetByTaskIdAsync(task.Id);
            Assert.Contains(fetched, c => c.Id == comment.Id);
        }
    }
}
