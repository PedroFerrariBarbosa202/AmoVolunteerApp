using Microsoft.Maui.Controls;
namespace VolunteerApp.Content.Pages.Login;

public partial class AccountLogPage : ContentPage {
    public AccountLogPage() {
        InitializeComponent();

        // return to home screen
        ToolbarItems.Add(new ToolbarItem {
            Text = "Voltar",
            Command = new Command(async () =>
                await Shell.Current.GoToAsync("//MainPage"))
        });
    }

    public async void GoToLogin(object sender, EventArgs e) {
        await Navigation.PushAsync(new LoginPage());
    }

    public async void GoToSignin(object sender, EventArgs e) {
        await Shell.Current.GoToAsync("//AccountDisconnected");
    }
}