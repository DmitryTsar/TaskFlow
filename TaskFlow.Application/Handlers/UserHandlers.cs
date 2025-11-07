using MediatR;
using TaskFlow.Application.Commands.UserCommand;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Queries.UserQueries;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Handlers
{
    public class UserHandlers :
        IRequestHandler<CreateUserCommand, UserDto>,
        IRequestHandler<UpdateUserCommand, UserDto?>,
        IRequestHandler<DeleteUserCommand, bool>,
        IRequestHandler<GetUserByIdQuery, UserDto?>,
        IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>,
        IRequestHandler<GetUserByEmailQuery, UserDto?>,
        IRequestHandler<GetUserProjectsQuery, IEnumerable<ProjectDto>>
    {
        private readonly IUserRepository _userRepository;

        public UserHandlers(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // 🟢 Create
        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var passwordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(request.Password));

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = passwordHash,
                Role = UserRole.User
            };

            await _userRepository.AddAsync(user);

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role
            };
        }

        // 🟡 Update
        public async Task<UserDto?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var existing = await _userRepository.GetByIdAsync(request.Id);
            if (existing == null) return null;

            existing.UserName = request.UserName;
            existing.Email = request.Email;
            existing.Role = request.Role;

            await _userRepository.UpdateAsync(existing);

            return new UserDto
            {
                Id = existing.Id,
                UserName = existing.UserName,
                Email = existing.Email,
                Role = existing.Role
            };
        }

        // 🔴 Delete
        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            return await _userRepository.DeleteAsync(request.Id);
        }

        // 🔍 Get by Id
        public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            return user == null ? null : new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role
            };
        }

        // 📋 Get all
        public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Role = u.Role
            });
        }

        // 📧 Get by Email
        public async Task<UserDto?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            return user == null ? null : new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role
            };
        }
        public async Task<IEnumerable<ProjectDto>> Handle(GetUserProjectsQuery request, CancellationToken cancellationToken)
        {
            var projects = await _userRepository.GetUserProjectsAsync(request.UserId);

            return projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CreatedAt = p.CreatedAt
            });
        }
    }
}
