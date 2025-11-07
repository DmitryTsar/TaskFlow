using System;
using System.Linq;
using System.Threading.Tasks;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Repositories;
using Xunit;

namespace TaskFlow.Tests.RepositoryTests
{
    public class UserRepositoryTests
    {
        [Fact]
        public async Task AddAndGetUser_ShouldWork()
        {
            var context = TestDbContextFactory.CreateInMemory();
            var repo = new UserRepository(context);

            var user = new User { UserName = "testuser", Email = "test@example.com", PasswordHash = "hash" };
            await repo.AddAsync(user);

            var fetched = await repo.GetByIdAsync(user.Id);
            Assert.NotNull(fetched);
            Assert.Equal("testuser", fetched!.UserName);

            var allUsers = await repo.GetAllAsync();
            Assert.Contains(allUsers, u => u.Id == user.Id);
        }
    }
}
