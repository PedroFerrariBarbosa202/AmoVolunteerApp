using System.Diagnostics.Tracing;
using VolunteerApp.Services;
using VolunteerApp.Pages;
namespace VolunteerApp.Cards;

public partial class EventAddCard : ContentView {
	private Models.Events data;
    public EventAddCard(Models.Events evData) {
        InitializeComponent();
        data = evData;
        BindingContext = evData;
    }

    public async void GoToEvent(object sender, EventArgs e) {
        var button = (Button)sender;

        await button.ScaleTo(0.8, 60, Easing.Linear);
        await button.ScaleTo(1.0, 60, Easing.Linear);

        // go to next page
        await Navigation.PushAsync(new DescriptPage(data));
    }
}