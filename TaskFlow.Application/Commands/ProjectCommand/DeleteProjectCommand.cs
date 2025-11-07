using MediatR;
using System;

namespace TaskFlow.Application.Commands
{
    public class DeleteProjectCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
