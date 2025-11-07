using Microsoft.Maui.Controls;
using TaskFlow.Client.Models;
using TaskFlow.Client.Services;

namespace TaskFlow.Client.Views;

public partial class RegisterPage : ContentPage
{
    private readonly AuthService _authService = new();

    public RegisterPage()
    {
        InitializeComponent();
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        var register = new RegisterRequest
        {
            UserName = UserNameEntry.Text,
            Email = EmailEntry.Text,
            Password = PasswordEntry.Text
        };

        var authResponse = await _authService.RegisterAsync(register);
        if (authResponse != null)
        {
            await DisplayAlert("Success", "Registration complete", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Error", "Registration failed", "OK");
        }
    }
}
