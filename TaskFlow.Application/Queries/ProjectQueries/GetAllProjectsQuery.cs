using MediatR;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Queries.ProjectQueries
{
    public class GetAllProjectsQuery : IRequest<IEnumerable<ProjectDto>> { }
}
