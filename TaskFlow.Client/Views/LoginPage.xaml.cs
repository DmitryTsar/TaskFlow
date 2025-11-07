using Microsoft.Maui.Controls;
using TaskFlow.Client.Models;
using TaskFlow.Client.Services;

namespace TaskFlow.Client.Views;

public partial class LoginPage : ContentPage
{
    private readonly AuthService _authService = new();

    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        var login = new LoginRequest
        {
            Email = EmailEntry.Text,
            Password = PasswordEntry.Text
        };

        var authResponse = await _authService.LoginAsync(login);
        if (authResponse != null)
        {
            await NavigateByRole(authResponse.Role);
        }
        else
        {
            await DisplayAlert("Error", "Invalid login or password", "OK");
        }
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterPage());
    }

    private async Task NavigateByRole(UserRole role)
    {
        Page targetPage = role switch
        {
            UserRole.Admin => new AdminPage(),
            UserRole.Manager => new ManagerPage(),
            UserRole.User => new UserPage(),
            _ => throw new NotImplementedException()
        };

        await Navigation.PushAsync(targetPage);
    }
}
