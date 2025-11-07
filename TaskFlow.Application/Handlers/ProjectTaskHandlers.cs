using MediatR;
using TaskFlow.Application.Commands;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Interfaces;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.Handlers
{
    public class ProjectTaskHandlers :
        IRequestHandler<GetProjectTasksCommand, IEnumerable<TaskForProjectDto>>,
        IRequestHandler<GetProjectTasksByStatusCommand, IEnumerable<TaskForProjectDto>>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;

        public ProjectTaskHandlers(IProjectRepository projectRepository, ITaskRepository taskRepository)
        {
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskForProjectDto>> Handle(GetProjectTasksCommand request, CancellationToken cancellationToken)
        {
            var tasks = await _projectRepository.GetTasksAsync(request.ProjectId);

            return tasks.Select(t => new TaskForProjectDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                AssignedToId = t.AssignedToId ?? Guid.Empty, // безопасное присваивание
                AssignedToName = t.AssignedTo?.UserName ?? string.Empty,
                Status = t.Status.ToString(),
                CreatedAt = t.CreatedAt
            });
        }

        public async Task<IEnumerable<TaskForProjectDto>> Handle(GetProjectTasksByStatusCommand request, CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetByStatusAsync(request.ProjectId, request.Status);

            return tasks.Select(t => new TaskForProjectDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                AssignedToId = t.AssignedToId ?? Guid.Empty, // безопасное присваивание
                AssignedToName = t.AssignedTo?.UserName ?? string.Empty,
                Status = t.Status.ToString(),
                CreatedAt = t.CreatedAt
            });
        }
    }
}
