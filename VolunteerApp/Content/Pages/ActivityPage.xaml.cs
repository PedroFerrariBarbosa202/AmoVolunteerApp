using System.Globalization;
using VolunteerApp.Services;

namespace VolunteerApp.Pages;

public partial class ActivityPage : ContentPage
{
	public ActivityPage()
	{
		InitializeComponent();
    }

    protected override async void OnAppearing() {
        base.OnAppearing();

        // make it so user cant use fuctionality if not logged in
        if (!UserService.UService.IsLoggedIn()) {
            await DisplayAlert(
                    "Erro detectado:",
                    $"Por favor, crie uma conta para poder ter acesso a essa funcionalidade.",
                    "Continuar");
            await Shell.Current.GoToAsync($"///{nameof(MainPage)}");
            return;
        }

        // if is logged in, update UI
        UpdateUI();
    }

        async void OnAddActivityClicked(object sender, EventArgs e) {
        try {
            // add loading page
            OverlayManager.SetLoadingOverlay(ContentGrid);

            var current_user = AccountState.volunteerData;

            var volunteer = await DatabaseConnector.Client
                    .From<Models.Volunteer>()
                    .Where(v => v.email == current_user.email)
                    .Where(v => v.password == current_user.password)
                    .Single();

            var model = new Models.Activity();

            // get an activity that is not closed by user
            Models.Activity? activity = await GetOpenShift(volunteer.volunteer_ID);

            // update it properly
            if (activity == null) {
                model = new Models.Activity {
                    volunteer_ID = volunteer.volunteer_ID,
                    created_at = DateTime.Now,
                };
                await DatabaseConnector.Client.From<Models.Activity>().Insert(model);
            }
            else {
                await DatabaseConnector.Client
                    .From<Models.Activity>()
                    .Where(x => x.ID == activity.ID)
                    .Set(x => x.finished_at, DateTime.Now)
                    .Update();
            }
            UpdateUI();

            // remove loading page
            OverlayManager.RemoveLoadingOverlay(ContentGrid);
        }
        catch (Exception ex) {
            await DisplayAlert(
                    "Erro detectado:",
                    $"{ex.Message}",
                    "Continuar");
            await Shell.Current.GoToAsync($"///{nameof(MainPage)}");
        }
    }

    void OnChangeDate(object sender, DateChangedEventArgs e) {
        UpdateUI();
    }

    async void UpdateUI() {
        try {
            // default text so user knows a process is happening
            StatusLabel.Text = "Validando Dados...";
            StatusLabel.TextColor = Colors.White;

            HourLabel.Text = DateTime.Now.ToString("dd/MM/yyyy");
            TodayLabel.Text = DateTime.Now.ToString("dddd", new CultureInfo("pt-BR"));

            var is_active = await IsActiveOnDate();

            var current_user = AccountState.volunteerData;
            if (current_user == null) {
                throw new Exception("Voluntário não encontrado.");
            }

            var volunteer = await DatabaseConnector.Client
                .From<Models.Volunteer>()
                .Where(v => v.email == current_user.email)
                .Where(v => v.password == current_user.password)
                .Single();

            var open_shift = await GetOpenShift(volunteer.volunteer_ID);

            if (is_active && open_shift != null) {
                StatusLabel.Text = "Em Atividade";
                StatusLabel.TextColor = Colors.Yellow;
            }
            else if (is_active && open_shift == null) {
                StatusLabel.Text = "Atividade Finalizada";
                StatusLabel.TextColor = Colors.Green;
                SetButton(false);
            }
            else {
                StatusLabel.Text = "Ausente";
                StatusLabel.TextColor = Colors.Red;
            }
        } catch (Exception ex) {
            await DisplayAlert(
                        "Erro detectado:",
                        $"{ex}",
                        "Continuar");
            await Shell.Current.GoToAsync($"///{nameof(MainPage)}");
            return;
        }
    }

    void SetButton(bool is_enabled) {
        ActivityTrigger.IsEnabled = is_enabled;
        ActivityTrigger.BackgroundColor = is_enabled ? Colors.Green : Colors.Gray;
    }

    // returns if the volunteer was active in a specific day
    async Task<bool> IsActiveOnDate() {
        var startOfDay = DateTime.Now.Date;             
        var endOfDay = startOfDay.AddDays(1);    

        var result = await DatabaseConnector.Client
            .From<Models.Activity>()
            .Where(x => x.created_at >= startOfDay &&
                        x.created_at < endOfDay)
            .Get();

        return result.Models.Count > 0;
    }

    async Task<Models.Activity?> GetOpenShift(int volunteerId) {
        var startOfDay = DateTime.Now.Date;
        var endOfDay = startOfDay.AddDays(1);

        var result = await DatabaseConnector.Client
            .From<Models.Activity>()
            .Where(x => x.volunteer_ID == volunteerId)
            .Where(x => x.created_at >= startOfDay &&
                        x.created_at < endOfDay)
            .Where(x => x.finished_at == null)
            .Single();

        return result;
    }
}