// 📁 TaskFlow.Application/Queries/UserQueries/GetUserProjectsQuery.cs
using MediatR;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Queries.UserQueries
{
    public record GetUserProjectsQuery(Guid UserId) : IRequest<IEnumerable<ProjectDto>>;
}
