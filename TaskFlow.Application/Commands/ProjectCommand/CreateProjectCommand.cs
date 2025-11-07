using MediatR;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Commands
{
    public class CreateProjectCommand : IRequest<ProjectDto>
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public Guid OwnerId { get; set; }
    }
}
