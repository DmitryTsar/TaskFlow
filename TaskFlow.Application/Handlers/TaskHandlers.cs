using MediatR;
using System.Threading.Channels;
using TaskFlow.Application.Commands.TaskCommand;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Queries.TaskQueries;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Handlers
{
    public class TaskHandlers :
        IRequestHandler<CreateTaskCommand, TaskDto>,
        IRequestHandler<UpdateTaskCommand, TaskDto>,
        IRequestHandler<DeleteTaskCommand, bool>,
        IRequestHandler<GetTaskByIdQuery, TaskDto>,
        IRequestHandler<GetTasksByProjectQuery, IEnumerable<TaskDto>>,
        IRequestHandler<GetTasksByStatusQuery, IEnumerable<TaskDto>>,
        IRequestHandler<GetAllTasksQuery, IEnumerable<TaskDto>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskHistoryRepository _taskHistoryRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;

        public TaskHandlers(ITaskRepository taskRepository, ITaskHistoryRepository taskHistoryRepository, IProjectRepository projectRepository, IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _taskHistoryRepository = taskHistoryRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
        }

        public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = new TaskItem
            {
                Title = request.Title,
                Description = request.Description,
                ProjectId = request.ProjectId,
                CreatedById = request.CreatedById,
                AssignedToId = request.AssignedToId,
                Status = request.Status,
                CreatedAt = DateTime.UtcNow
            };

            await _taskRepository.AddAsync(task);

            var project = await _projectRepository.GetByIdAsync(task.ProjectId);
            var createdBy = await _userRepository.GetByIdAsync(task.CreatedById);
            var assignedTo = task.AssignedToId.HasValue ? await _userRepository.GetByIdAsync(task.AssignedToId.Value) : null;

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                ProjectId = task.ProjectId,
                ProjectName = project?.Name ?? string.Empty,
                CreatedById = task.CreatedById,
                CreatedByName = createdBy?.UserName ?? string.Empty,
                AssignedToId = task.AssignedToId,
                AssignedToName = assignedTo?.UserName ?? string.Empty,
                Status = task.Status,
                CreatedAt = task.CreatedAt
            };
        }

        public async Task<TaskDto> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var existing = await _taskRepository.GetByIdAsync(request.Id);
            if (existing == null) throw new Exception("Task not found");

            var changes = new List<TaskHistory>();

            void AddChange(string prop, string? oldVal, string? newVal)
            {
                changes.Add(new TaskHistory
                {
                    TaskId = existing.Id,
                    PropertyName = prop,
                    OldValue = oldVal,
                    NewValue = newVal,
                    ChangedById = request.UpdatedById,
                    ChangedAt = DateTime.UtcNow
                });
            }
            // Проверяем изменения и фиксируем историю
            if (existing.Title != request.Title)
            {
                AddChange("Title", existing.Title, request.Title);
                existing.Title = request.Title;
            }

            if (existing.Description != request.Description)
            {
                AddChange("Description", existing.Description, request.Description);
                existing.Description = request.Description;
            }

            if (existing.Status != request.Status)
            {
                AddChange("Status", existing.Status.ToString(), request.Status.ToString());
                existing.Status = request.Status;
            }

            if (existing.AssignedToId != request.AssignedToId)
            {
                AddChange("AssignedToId", existing.AssignedToId?.ToString(), request.AssignedToId?.ToString());
                existing.AssignedToId = request.AssignedToId;
            }
            //existing.Title = request.Title;
            //existing.Description = request.Description;
            //existing.AssignedToId = request.AssignedToId;
            //existing.Status = request.Status;


            await _taskRepository.UpdateAsync(existing);

            // Если есть изменения — записываем историю
            foreach (var change in changes)
                await _taskHistoryRepository.AddAsync(change);

            var project = await _projectRepository.GetByIdAsync(existing.ProjectId);
            var createdBy = await _userRepository.GetByIdAsync(existing.CreatedById);
            var assignedTo = existing.AssignedToId.HasValue ? await _userRepository.GetByIdAsync(existing.AssignedToId.Value) : null;

            return new TaskDto
            {
                Id = existing.Id,
                Title = existing.Title,
                Description = existing.Description,
                ProjectId = existing.ProjectId,
                ProjectName = project?.Name ?? string.Empty,
                CreatedById = existing.CreatedById,
                CreatedByName = createdBy?.UserName ?? string.Empty,
                AssignedToId = existing.AssignedToId,
                AssignedToName = assignedTo?.UserName ?? string.Empty,
                Status = existing.Status,
                CreatedAt = existing.CreatedAt
            };
        }

        public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            return await _taskRepository.DeleteAsync(request.Id);
        }

        public async Task<TaskDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetFullTaskAsync(request.Id);
            if (task == null) throw new Exception("Task not found");

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                ProjectId = task.ProjectId,
                ProjectName = task.Project?.Name ?? string.Empty,
                CreatedById = task.CreatedById,
                CreatedByName = task.CreatedBy?.UserName ?? string.Empty,
                AssignedToId = task.AssignedToId,
                AssignedToName = task.AssignedTo?.UserName ?? string.Empty,
                Status = task.Status,
                CreatedAt = task.CreatedAt
            };
        }

        public async Task<IEnumerable<TaskDto>> Handle(GetTasksByProjectQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetByProjectAsync(request.ProjectId);
            return await MapTasksToDto(tasks);
        }
        public async Task<IEnumerable<TaskDto>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetAllAsync(); // должен быть метод в репозитории
            return await MapTasksToDto(tasks);
        }
        public async Task<IEnumerable<TaskDto>> Handle(GetTasksByStatusQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetByStatusAsync(request.ProjectId, request.Status);
            return await MapTasksToDto(tasks);
        }

        private async Task<IEnumerable<TaskDto>> MapTasksToDto(IEnumerable<TaskItem> tasks)
        {
            var list = new List<TaskDto>();
            foreach (var task in tasks)
            {
                var project = await _projectRepository.GetByIdAsync(task.ProjectId);
                var createdBy = await _userRepository.GetByIdAsync(task.CreatedById);
                var assignedTo = task.AssignedToId.HasValue ? await _userRepository.GetByIdAsync(task.AssignedToId.Value) : null;

                list.Add(new TaskDto
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    ProjectId = task.ProjectId,
                    ProjectName = project?.Name ?? string.Empty,
                    CreatedById = task.CreatedById,
                    CreatedByName = createdBy?.UserName ?? string.Empty,
                    AssignedToId = task.AssignedToId,
                    AssignedToName = assignedTo?.UserName ?? string.Empty,
                    Status = task.Status,
                    CreatedAt = task.CreatedAt
                });
            }
            return list;
        }
    }
}
