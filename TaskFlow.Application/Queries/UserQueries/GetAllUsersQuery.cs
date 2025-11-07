using MediatR;
using TaskFlow.Application.DTOs;
using System.Collections.Generic;

namespace TaskFlow.Application.Queries.UserQueries
{
    public class GetAllUsersQuery : IRequest<IEnumerable<UserDto>> { }
}
