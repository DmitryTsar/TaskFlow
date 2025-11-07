using MediatR;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.Commands.UserCommand
{
    public class UpdateUserCommand : IRequest<UserDto?>
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public UserRole Role { get; set; }
    }
}
