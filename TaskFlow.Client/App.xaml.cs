using Microsoft.Maui.Controls;
using TaskFlow.Client.Views;

namespace TaskFlow.Client;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new NavigationPage(new LoginPage());
    }
}
