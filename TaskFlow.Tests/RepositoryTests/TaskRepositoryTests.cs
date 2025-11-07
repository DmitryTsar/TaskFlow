using System;
using System.Linq;
using System.Threading.Tasks;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;
using TaskFlow.Infrastructure.Repositories;
using Xunit;
using TaskStatus = TaskFlow.Domain.Enums.TaskStatus;

namespace TaskFlow.Tests.RepositoryTests
{
    public class TaskRepositoryTests
    {
        [Fact]
        public async Task AddAndGetFullTask_ShouldWork()
        {
            var context = TestDbContextFactory.CreateInMemory();
            var user = new User { UserName = "creator", Email = "creator@test.com", PasswordHash = "hash" };
            var project = new Project { Name = "Proj", Owner = user };
            context.Users.Add(user);
            context.Projects.Add(project);
            await context.SaveChangesAsync();

            var repo = new TaskRepository(context);
            var task = new TaskItem
            {
                Title = "Test Task",
                ProjectId = project.Id,
                CreatedById = user.Id,
                Status = TaskStatus.New
            };
            await repo.AddAsync(task);

            var fullTask = await repo.GetFullTaskAsync(task.Id);
            Assert.NotNull(fullTask);
            Assert.Equal("Test Task", fullTask!.Title);
            Assert.Equal(project.Id, fullTask.ProjectId);
            Assert.Equal(user.Id, fullTask.CreatedById);
        }
    }
}
