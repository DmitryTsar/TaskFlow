using System;
using System.Linq;
using System.Threading.Tasks;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Repositories;
using Xunit;

namespace TaskFlow.Tests.RepositoryTests
{
    public class ProjectRepositoryTests
    {
        [Fact]
        public async Task AddAndGetProject_ShouldWork()
        {
            var context = TestDbContextFactory.CreateInMemory();
            var user = new User { UserName = "owner", Email = "owner@test.com", PasswordHash = "hash" };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var repo = new ProjectRepository(context);
            var project = new Project { Name = "Test Project", OwnerId = user.Id };
            await repo.AddAsync(project);

            var fetched = await repo.GetByIdAsync(project.Id);
            Assert.NotNull(fetched);
            Assert.Equal("Test Project", fetched!.Name);
            Assert.Equal(user.Id, fetched.OwnerId);
        }
    }
}
