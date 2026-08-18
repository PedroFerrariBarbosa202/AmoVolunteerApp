using VolunteerApp.Services;
using VolunteerApp.Cards;
namespace VolunteerApp.Pages;

public partial class SecondPage : ContentPage {
    public SecondPage() {
        InitializeComponent();

        // return to home screen
        ToolbarItems.Add(new ToolbarItem {
            Text = "Gerenciar Eventos",
            Command = new Command(async () =>
                await Shell.Current.GoToAsync("//ManagePage"))
        });

        ToolbarItems.Add(new ToolbarItem {
            Text = "Voltar",
            Command = new Command(async () =>
                await Shell.Current.GoToAsync("//MainPage"))
        });
    }

    protected override void OnAppearing() {
        base.OnAppearing();
        ClearEvents();
        ShowEvents();
    }

    private async void ShowEvents() {
        // add loading page
        OverlayManager.SetLoadingOverlay(ContentGrid);

        // convert query to command
        await DatabaseConnector.InitializeAsync();
        var response = await DatabaseConnector.Client
            .From<Models.Events>()
            .Get();

        List<Models.Events> events = response.Models;

        foreach(Models.Events row in events) {
            var card = new EventAddCard(row);
            EventStackLayout.Children.Add(card);
        }

        OverlayManager.RemoveLoadingOverlay(ContentGrid);
    }

    void ClearEvents() {
        foreach (var child in EventStackLayout.Children.ToList()) {
            EventStackLayout.Children.Remove(child);
        }
    }
}