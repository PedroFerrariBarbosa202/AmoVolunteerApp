
namespace VolunteerApp.Pages;

public partial class SettingsPage : ContentPage {
    public SettingsPage() {
        InitializeComponent();
        LoadSettings();
    }

    private void LoadSettings() {
        bool isDark = Preferences.Get("DarkMode", true);
        bool followSystem = Preferences.Get("FollowSystem", false);

        DarkModeSwitch.IsToggled = isDark;
        SystemThemeSwitch.IsToggled = followSystem;

        ApplyTheme(isDark, followSystem);
    }

    private void OnDarkModeToggled(object sender, ToggledEventArgs e) {
        Preferences.Set("DarkMode", e.Value);
        ApplyTheme(e.Value, SystemThemeSwitch.IsToggled);
    }

    private void OnSystemThemeToggled(object sender, ToggledEventArgs e) {
        Preferences.Set("FollowSystem", e.Value);
        ApplyTheme(DarkModeSwitch.IsToggled, e.Value);
    }

    private void ApplyTheme(bool isDark, bool followSystem) {
        // bizarre
        Application.Current.UserAppTheme = followSystem ? AppTheme.Unspecified : (isDark ? AppTheme.Dark : AppTheme.Light);
    }
}