using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}
