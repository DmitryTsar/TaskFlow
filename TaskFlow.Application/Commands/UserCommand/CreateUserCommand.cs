using MediatR;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Commands.UserCommand
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
