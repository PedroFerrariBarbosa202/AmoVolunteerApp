using Azure;
using Microsoft.Maui.Controls.PlatformConfiguration;
using VolunteerApp.Pages;
using VolunteerApp.Services;

namespace VolunteerApp.Content.Pages.Login;

public partial class SigninConfirmationPage : ContentPage
{
    int checks = 0;
	public SigninConfirmationPage()
	{
		InitializeComponent();

        // return to home screen
        ToolbarItems.Add(new ToolbarItem {
            Text = "Voltar",
            Command = new Command(async () =>
                await Shell.Current.GoToAsync("//MainPage"))
        });
    }

    void OnCheckBoxChanged(object sender, CheckedChangedEventArgs e) {
        bool isChecked = e.Value;

        // check manually if both checkboxes are clicked
        // probably not best method :(
        if (isChecked) {
            checks++;
        }
        else {
            checks--;
        }

        if(checks >= 2) {
            SendButton.BackgroundColor = Colors.Green;
            SendButton.IsEnabled = true;
        }
        else {
            SendButton.BackgroundColor = Colors.Gray;
            SendButton.IsEnabled = false;
        }
    }

    async void AddSolicitation(object sender, EventArgs e) {
        AddUserAsync();

        await DisplayAlert("Resultado de sign in", "Solicitação criada com sucesso!", "Continuar");
        await Shell.Current.GoToAsync("//ContactPage");
    }

    private async void AddUserAsync() {
        try {
            var buffer = await DatabaseConnector.Client.From<Models.Volunteer>().Insert(AccountState.volunteerData);
            var newUser = buffer.Models.FirstOrDefault();

            // add sector connections to new solicitation 
            foreach (var sector in AccountState.sectors) {
                var connection_model = new Models.VolunteerSector {
                    volunteer_ID = newUser.volunteer_ID,
                    sector_ID = sector.sector_ID,
                    is_validated = true,
                };

                var sector_response = await DatabaseConnector.Client
                .From<Models.VolunteerSector>()
                .Insert(connection_model);
            }

            await UserService.UService.SetCurrentUserIDAsync(AccountState.volunteerData.volunteer_ID);
        }
        catch (Exception ex) {
            await DisplayAlert("Resultado de sign in", $"Problema identificado ao criar conta: {ex.Message}", "Continuar");
        }
    }
}