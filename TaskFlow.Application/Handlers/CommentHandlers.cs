using MediatR;
using TaskFlow.Application.Commands;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Queries;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Handlers
{
    public class CommentHandlers :
        IRequestHandler<CreateCommentCommand, CommentDto>,
        IRequestHandler<UpdateCommentCommand, CommentDto?>,
        IRequestHandler<DeleteCommentCommand, bool>,
        IRequestHandler<GetAllCommentsQuery, IEnumerable<CommentDto>>,
        IRequestHandler<GetCommentByIdQuery, CommentDto?>,
        IRequestHandler<GetCommentsByTaskIdQuery, IEnumerable<CommentDto>>
    {
        private readonly ICommentRepository _repository;

        public CommentHandlers(ICommentRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommentDto> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var entity = new Comment
            {
                Content = request.Content,
                AuthorId = request.AuthorId,
                TaskId = request.TaskId
            };

            var added = await _repository.AddAsync(entity);

            return new CommentDto
            {
                Id = added.Id,
                Content = added.Content,
                CreatedAt = added.CreatedAt,
                AuthorId = added.AuthorId,
                AuthorName = added.Author?.UserName ?? "",
                TaskId = added.TaskId,
                TaskTitle = added.Task?.Title ?? ""
            };
        }

        public async Task<CommentDto?> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);
            if (entity == null) return null;

            entity.Content = request.Content;
            var updated = await _repository.UpdateAsync(entity);
            if (updated == null) return null;

            return new CommentDto
            {
                Id = updated.Id,
                Content = updated.Content,
                CreatedAt = updated.CreatedAt,
                AuthorId = updated.AuthorId,
                AuthorName = updated.Author?.UserName ?? "",
                TaskId = updated.TaskId,
                TaskTitle = updated.Task?.Title ?? ""
            };
        }

        public async Task<bool> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteAsync(request.Id);
        }

        public async Task<IEnumerable<CommentDto>> Handle(GetAllCommentsQuery request, CancellationToken cancellationToken)
        {
            var comments = await _repository.GetAllAsync();
            return comments.Select(c => new CommentDto
            {
                Id = c.Id,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                AuthorId = c.AuthorId,
                AuthorName = c.Author.UserName,
                TaskId = c.TaskId,
                TaskTitle = c.Task.Title
            });
        }

        public async Task<CommentDto?> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
        {
            var c = await _repository.GetByIdAsync(request.Id);
            if (c == null) return null;

            return new CommentDto
            {
                Id = c.Id,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                AuthorId = c.AuthorId,
                AuthorName = c.Author.UserName,
                TaskId = c.TaskId,
                TaskTitle = c.Task.Title
            };
        }

        public async Task<IEnumerable<CommentDto>> Handle(GetCommentsByTaskIdQuery request, CancellationToken cancellationToken)
        {
            var comments = await _repository.GetByTaskIdAsync(request.TaskId);
            return comments.Select(c => new CommentDto
            {
                Id = c.Id,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                AuthorId = c.AuthorId,
                AuthorName = c.Author.UserName,
                TaskId = c.TaskId,
                TaskTitle = c.Task.Title
            });
        }
    }
}
