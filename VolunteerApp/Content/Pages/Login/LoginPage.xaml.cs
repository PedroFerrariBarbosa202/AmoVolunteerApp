using VolunteerApp.Services;
namespace VolunteerApp.Content.Pages.Login;

public partial class LoginPage : ContentPage {
    public LoginPage() {
        InitializeComponent();
    }

    public async void LoginUser(object sender, EventArgs e) {
        ClickAnim(sender);

        // add loading page
        OverlayManager.SetLoadingOverlay(ContentGrid);

        await DatabaseConnector.InitializeAsync();
        try {
            var volunteer = await DatabaseConnector.Client
                .From<Models.Volunteer>()
                .Where(v => v.email == EmailEntry.Text)
                .Where(v => v.password == PasswordEntry.Text)
                .Single();

            if(volunteer == null) {
                await DisplayAlert(
                "Erro na recuperação de dados",
                "Email ou senha incorretos.",
                "Concluir");

                OverlayManager.RemoveLoadingOverlay(ContentGrid);

                return;
            }

            if(volunteer.is_validated == false) {
                await DisplayAlert(
                "Erro na recuperação de dados",
                "A conta que queres se conectar ainda não foi verificada pelos moderadores.",
                "Concluir");

                OverlayManager.RemoveLoadingOverlay(ContentGrid);

                return;
            }

            await UserService.UService.SetCurrentUserIDAsync(volunteer.volunteer_ID);
            var response = await DatabaseConnector.Client
                .From<Models.Volunteer>()
                .Single();

            AccountState.volunteerData = response;
            AccountState.volunteerData.logged_in = true;

            // display sucess message
            await DisplayAlert(
            "Conectado com sucesso na conta!",
            $"Bem vindo(a) {volunteer.name}",
            "Concluir");

            OverlayManager.RemoveLoadingOverlay(ContentGrid);

            // voltar para MainPage
            await Shell.Current.GoToAsync("//MainPage");
        }
        catch (Exception ex) {
            await DisplayAlert(
            "Resultado de login",
            ex.Message,
            "Continuar");

            OverlayManager.RemoveLoadingOverlay(ContentGrid);

            return;
        }

        OverlayManager.RemoveLoadingOverlay(ContentGrid);
    }

    async void ClickAnim(object sender) {
        if (sender is Button btn) {
            await btn.ScaleTo(0.9, 100, Easing.CubicOut);
            await btn.ScaleTo(1.0, 100, Easing.CubicOut);
        }
    }
}