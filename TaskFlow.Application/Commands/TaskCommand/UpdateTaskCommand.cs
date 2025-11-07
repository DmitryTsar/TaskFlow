using MediatR;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Enums;

public class UpdateTaskCommand : IRequest<TaskDto>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public Guid? AssignedToId { get; set; }
    public TaskFlow.Domain.Enums.TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    //кто изменил задачу
    public Guid UpdatedById { get; set; }
}


