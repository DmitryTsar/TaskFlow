using TaskFlow.Client.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace TaskFlow.Client.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;

    public AuthService()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:5001/") };
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        return null;
    }

    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", request);
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        return null;
    }
}
