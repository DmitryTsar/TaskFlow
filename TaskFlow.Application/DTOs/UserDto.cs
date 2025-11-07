using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public UserRole Role { get; set; }
    }
}
