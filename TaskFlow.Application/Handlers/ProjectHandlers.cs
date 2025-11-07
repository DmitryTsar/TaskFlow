using MediatR;
using TaskFlow.Application.Commands;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Handlers
{
    public class ProjectHandlers :
        IRequestHandler<CreateProjectCommand, ProjectDto>,
        IRequestHandler<UpdateProjectCommand, ProjectDto>,
        IRequestHandler<DeleteProjectCommand, bool>
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectHandlers(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = new Project
            {
                Name = request.Name,
                Description = request.Description,
                OwnerId = request.OwnerId
            };

            var created = await _projectRepository.AddAsync(project);

            // Безопасно подгружаем Owner
            var owner = (await _projectRepository.GetByIdAsync(created.Id))?.Owner;

            return new ProjectDto
            {
                Id = created.Id,
                Name = created.Name,
                Description = created.Description,
                CreatedAt = created.CreatedAt,
                OwnerId = created.OwnerId,
                OwnerName = owner?.UserName ?? string.Empty // безопасный доступ
            };
        }

        public async Task<ProjectDto> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var existing = await _projectRepository.GetByIdAsync(request.Id);
            if (existing == null) throw new Exception("Проект не найден");

            existing.Name = request.Name;
            existing.Description = request.Description;

            var updated = await _projectRepository.UpdateAsync(existing);

            // Подгружаем Owner после обновления
            var owner = updated?.Owner;

            return new ProjectDto
            {
                Id = updated!.Id,
                Name = updated.Name,
                Description = updated.Description,
                CreatedAt = updated.CreatedAt,
                OwnerId = updated.OwnerId,
                OwnerName = owner?.UserName ?? string.Empty
            };
        }

        public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            return await _projectRepository.DeleteAsync(request.Id);
        }
    }
}
