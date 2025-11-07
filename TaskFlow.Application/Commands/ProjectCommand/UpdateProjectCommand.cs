using MediatR;
using TaskFlow.Application.DTOs;
using System;

namespace TaskFlow.Application.Commands
{
    public class UpdateProjectCommand : IRequest<ProjectDto>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}
