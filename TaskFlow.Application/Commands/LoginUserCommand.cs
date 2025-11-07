using MediatR;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Commands
{
    public class LoginUserCommand : IRequest<AuthResponseDto>
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
