using System.Security.Cryptography;
using System.Text;
using MediatR;
using TaskFlow.Application.Commands;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Handlers
{
    public class AuthHandlers :
        IRequestHandler<RegisterUserCommand, AuthResponseDto>,
        IRequestHandler<LoginUserCommand, AuthResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;

        public AuthHandlers(IUserRepository userRepository, IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
        }

        public async Task<AuthResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.EmailExistsAsync(request.Email))
                throw new Exception("Пользователь с таким email уже существует.");

            var passwordHash = HashPassword(request.Password);

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = passwordHash,
                Role = UserRole.User
            };

            await _userRepository.AddAsync(user);
            var token = _jwtProvider.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }

        public async Task<AuthResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email)
                ?? throw new Exception("Неверный email или пароль.");

            if (!VerifyPassword(request.Password, user.PasswordHash))
                throw new Exception("Неверный email или пароль.");

            var token = _jwtProvider.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private static bool VerifyPassword(string password, string hash) =>
            HashPassword(password) == hash;
    }
}
