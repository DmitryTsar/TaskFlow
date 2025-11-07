using MediatR;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Queries.UserQueries
{
    public class GetUserByIdQuery : IRequest<UserDto?>
    {
        public Guid Id { get; set; }

        public GetUserByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
