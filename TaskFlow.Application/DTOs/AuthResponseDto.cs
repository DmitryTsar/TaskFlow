namespace TaskFlow.Application.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Role { get; set; } = default!;

    }
}
