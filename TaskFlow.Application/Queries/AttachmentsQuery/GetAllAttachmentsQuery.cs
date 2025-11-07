using MediatR;
using TaskFlow.Application.DTOs;
using System.Collections.Generic;

namespace TaskFlow.Application.Queries
{
    public class GetAllAttachmentsQuery : IRequest<IEnumerable<AttachmentDto>> { }
}
