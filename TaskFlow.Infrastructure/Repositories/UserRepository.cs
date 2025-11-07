using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;
using TaskFlow.Infrastructure.Persistence;

namespace TaskFlow.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TaskFlowDbContext _context;

        public UserRepository(TaskFlowDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.OwnedProjects)
                .Include(u => u.CreatedTasks)
                .Include(u => u.AssignedTasks)
                .Include(u => u.Comments)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users
                .Include(u => u.OwnedProjects)
                .Include(u => u.CreatedTasks)
                .Include(u => u.AssignedTasks)
                .Include(u => u.Comments)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> UpdateAsync(User user)
        {
            var existing = await _context.Users.FindAsync(user.Id);
            if (existing == null) return null;

            _context.Entry(existing).CurrentValues.SetValues(user);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Project>> GetUserProjectsAsync(Guid userId)
        {
            return await _context.Projects
                .Where(p => p.OwnerId == userId)
                .Include(p => p.Tasks)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetCreatedTasksAsync(Guid userId)
        {
            return await _context.Tasks
                .Where(t => t.CreatedById == userId)
                .Include(t => t.Project)
                .Include(t => t.AssignedTo)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetAssignedTasksAsync(Guid userId)
        {
            return await _context.Tasks
                .Where(t => t.AssignedToId == userId)
                .Include(t => t.Project)
                .Include(t => t.CreatedBy)
                .ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByUserNameAsync(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
    }
}
