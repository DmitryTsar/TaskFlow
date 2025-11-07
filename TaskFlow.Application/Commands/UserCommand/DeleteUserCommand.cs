using MediatR;

namespace TaskFlow.Application.Commands.UserCommand
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
