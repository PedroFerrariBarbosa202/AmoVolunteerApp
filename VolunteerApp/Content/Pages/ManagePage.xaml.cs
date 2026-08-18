using VolunteerApp.Services;
using VolunteerApp.Cards;

namespace VolunteerApp.Pages;

public partial class ManagePage : ContentPage
{
    public ManagePage()
    {
        InitializeComponent();

        ToolbarItems.Add(new ToolbarItem {
            Text = "Voltar",
            Command = new Command(async () =>
                await Shell.Current.GoToAsync("//EventPage"))
        });
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ClearEvents();
        ShowEvents();
    }

    public async void ShowEvents()
    {
        // get id of volunteer from email
        await DatabaseConnector.InitializeAsync();

        try {
            var vol_id = await DatabaseConnector.Client
                .From<Models.Volunteer>()
                .Where(v => v.email == AccountState.volunteerData.email)
                .Single();

            // get events that volunteer is registered in
            var vol_event = await DatabaseConnector.Client
                .From<Models.VolunteerEvent>()
                .Where(v => v.volunteer_ID == vol_id.volunteer_ID)
                .Get();

            var EventsEntered = new HashSet<int>();
            foreach (Models.VolunteerEvent row in vol_event.Models) {
                // get event with specified id
                var ev = await DatabaseConnector.Client
                    .From<Models.Events>()
                    .Where(v => v.event_ID == row.event_ID)
                    .Single();

                if (EventsEntered.Contains(ev.event_ID))
                    continue;

                var eventManageCard = new EventManageCard(ev);
                EventStackLayout.Children.Add(eventManageCard);
            }

            string text = vol_event.Models.Any() ? " " : "Você não está inscrito em nenhum evento";
            LoadLabel.Text = text;
            LoadLabel.FontSize = 20;
        } catch(Exception ex) {
            await DisplayAlert(
                "Erro na recuperação de dados",
                ex.Message,
                "Concluir");
            return;
        }
    }

    public void ClearEvents() {
        foreach (var child in EventStackLayout.Children.ToList()) {
            EventStackLayout.Children.Remove(child);
        }
    }
}
