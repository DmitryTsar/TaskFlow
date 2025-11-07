using MediatR;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Queries.TaskQueries
{
    public class GetTaskHistoryByTaskIdQuery : IRequest<IEnumerable<TaskHistoryDto>>
    {
        public Guid TaskId { get; }

        public GetTaskHistoryByTaskIdQuery(Guid taskId)
        {
            TaskId = taskId;
        }
    }
}
