// 📁 TaskFlow.Application/Queries/UserQueries/GetUserAssignedTasksQuery.cs
using MediatR;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Queries.UserQueries
{
    public record GetUserAssignedTasksQuery(Guid UserId) : IRequest<IEnumerable<TaskDto>>;
}
