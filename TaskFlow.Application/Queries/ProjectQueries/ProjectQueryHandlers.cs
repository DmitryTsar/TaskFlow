using MediatR;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Queries.ProjectQueries;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Handlers
{
    public class ProjectQueryHandlers :
        IRequestHandler<GetAllProjectsQuery, IEnumerable<ProjectDto>>,
        IRequestHandler<GetProjectByIdQuery, ProjectWithTasksDto?>
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectQueryHandlers(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<ProjectDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            var projects = await _projectRepository.GetAllAsync();
            return projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CreatedAt = p.CreatedAt,
                OwnerId = p.OwnerId,
                OwnerName = p.Owner.UserName
            });
        }

        public async Task<ProjectWithTasksDto?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetProjectWithTasksAsync(request.Id);
            if (project == null) return null;

            return new ProjectWithTasksDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CreatedAt = project.CreatedAt,
                OwnerId = project.OwnerId,
                OwnerName = project.Owner.UserName,
                Tasks = project.Tasks
            };
        }
    }
}
