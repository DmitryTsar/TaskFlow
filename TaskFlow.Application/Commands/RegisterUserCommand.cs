using MediatR;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Commands
{
    public class RegisterUserCommand : IRequest<AuthResponseDto>
    {
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
