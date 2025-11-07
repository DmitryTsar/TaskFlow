using MediatR;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Queries.UserQueries
{
    public class GetUserByEmailQuery : IRequest<UserDto?>
    {
        public string Email { get; set; } = default!;

        public GetUserByEmailQuery(string email)
        {
            Email = email;
        }
    }
}
