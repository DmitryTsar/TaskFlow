using Microsoft.EntityFrameworkCore;
using TaskFlow.Infrastructure.Persistence;

namespace TaskFlow.Tests.RepositoryTests
{
    public static class TestDbContextFactory
    {
        public static TaskFlowDbContext CreateInMemory()
        {
            var options = new DbContextOptionsBuilder<TaskFlowDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new TaskFlowDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }
    }
}
