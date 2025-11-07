using MediatR;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Queries.ProjectQueries
{
    public class GetProjectByIdQuery : IRequest<ProjectWithTasksDto?>
    {
        public Guid Id { get; set; }
        public GetProjectByIdQuery(Guid id) => Id = id;
    }
}
