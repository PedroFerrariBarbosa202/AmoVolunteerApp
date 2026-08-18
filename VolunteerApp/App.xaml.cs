using System.Diagnostics;
using VolunteerApp.Pages; 
using VolunteerApp.Services;
namespace VolunteerApp
{
    public partial class App : Application {
        public App() {
            InitializeComponent();
            UserAppTheme = AppTheme.Dark;
        }

        protected override Window CreateWindow(IActivationState? activationState) {
            return new Window(new StartupPage());
        }

        public async Task InitializeUserAsync() {
            await DatabaseConnector.InitializeAsync();

            var user = await UserService.UService.GetCurrentUserIDAsync();

            if (user == null) {
                AccountState.volunteerData = AccountState.volunteerDefault;
                return;
            }

            await UserService.UService.SetCurrentUserIDAsync(user);

            var response = await DatabaseConnector.Client
                .From<Models.Volunteer>()
                .Single();

            AccountState.volunteerData = response;
            AccountState.volunteerData.logged_in = true;
        }
    }
}