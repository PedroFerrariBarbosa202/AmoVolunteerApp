namespace VolunteerApp.Pages;

public partial class StartupPage : ContentPage {
    private bool _initialized;

    public StartupPage() {
        InitializeComponent();
    }

    protected override void OnAppearing() {
        base.OnAppearing();
        InitializeComponent();

        if (_initialized)
            return;

        _initialized = true;

        _ = InitializeAppAsync();
    }

    private async Task InitializeAppAsync() {
        try {
            await ((App)Application.Current!).InitializeUserAsync();
            Application.Current!.Windows[0].Page = new AppShell();
            await Shell.Current.GoToAsync("//MainPage");
        }
        catch (Exception ex) {
            await DisplayAlert(
                "Erro",
                $"Não foi possível inicializar o aplicativo:\n{ex.Message}",
                "OK");

            _initialized = false;
        }
    }
}