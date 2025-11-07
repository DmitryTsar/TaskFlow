// 📁 TaskFlow.Application/Queries/UserQueries/GetUserCreatedTasksQuery.cs
using MediatR;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Queries.UserQueries
{
    public record GetUserCreatedTasksQuery(Guid UserId) : IRequest<IEnumerable<TaskDto>>;
}
