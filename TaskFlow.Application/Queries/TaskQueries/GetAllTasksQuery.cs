using MediatR;
using TaskFlow.Application.DTOs;
using System.Collections.Generic;

namespace TaskFlow.Application.Queries.TaskQueries
{
    public class GetAllTasksQuery : IRequest<IEnumerable<TaskDto>> { }
}
