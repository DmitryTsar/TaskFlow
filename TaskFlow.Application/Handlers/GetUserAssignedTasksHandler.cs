using MediatR;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Queries.UserQueries;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Handlers
{
    public class GetUserAssignedTasksHandler : IRequestHandler<GetUserAssignedTasksQuery, IEnumerable<TaskDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserAssignedTasksHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<TaskDto>> Handle(GetUserAssignedTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _userRepository.GetAssignedTasksAsync(request.UserId);

            return tasks.Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                ProjectId = t.ProjectId,
                ProjectName = t.Project?.Name ?? "Unknown",
                CreatedById = t.CreatedById,
                CreatedByName = t.CreatedBy?.UserName ?? "Unknown",
                AssignedToId = t.AssignedToId,
                AssignedToName = t.AssignedTo?.UserName ?? "Unassigned",
                Status = t.Status,
                //Priority = t.Priority,
                CreatedAt = t.CreatedAt
            });
        }
    }
}
