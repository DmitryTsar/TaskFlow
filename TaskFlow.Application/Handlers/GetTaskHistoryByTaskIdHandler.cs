using MediatR;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Queries;
using TaskFlow.Application.Queries.TaskQueries;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Handlers
{
    public class GetTaskHistoryByTaskIdHandler : IRequestHandler<GetTaskHistoryByTaskIdQuery, IEnumerable<TaskHistoryDto>>
    {
        private readonly ITaskHistoryRepository _repository;

        public GetTaskHistoryByTaskIdHandler(ITaskHistoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TaskHistoryDto>> Handle(GetTaskHistoryByTaskIdQuery request, CancellationToken cancellationToken)
        {
            var history = await _repository.GetByTaskIdAsync(request.TaskId);

            return history.Select(h => new TaskHistoryDto
            {
                PropertyName = h.PropertyName,
                OldValue = h.OldValue,
                NewValue = h.NewValue,
                ChangedByName = h.ChangedBy.UserName,
                ChangedAt = h.ChangedAt
            });
        }
    }
}
