using System.Data;
using VolunteerApp.Pages;
using VolunteerApp.Services;
namespace VolunteerApp.Cards;

public partial class EventManageCard : ContentView {
	private Models.Events data;

	public EventManageCard(Models.Events evData) {
		InitializeComponent();
		data = evData;
		BindingContext = evData;
	}
    public async void ExitEvent(object sender, EventArgs e) {
        //animate button 
        if (sender is Button btn) {
            await btn.ScaleTo(0.9, 100, Easing.CubicOut);
            await btn.ScaleTo(1.0, 100, Easing.CubicOut);
        }

        // get volunteer and then delete its connection to event
        await DatabaseConnector.InitializeAsync();
        var response = await DatabaseConnector.Client
            .From<Models.Volunteer>()
            .Where(v => v.email == AccountState.volunteerData.email)
            .Single();

        // add 1 to the role that hes in
        // since he is leaving, one more space could be ocuppied
        // get the specific role entry for this volunteer + event
        var events = await DatabaseConnector.Client
            .From<Models.VolunteerEvent>()
            .Where(v => v.volunteer_ID == response.volunteer_ID)
            .Where(v => v.event_ID == data.event_ID)
            .Get();

        foreach(var ev in events.Models) {
            // get every event role the user is connected to
            // needs to be called to capacity can be calculated afterwards
            var role = await DatabaseConnector.Client
                .From<Models.EventRole>()
                .Where(v => v.event_ID == data.event_ID)
                .Where(v => v.role_ID == ev.role_ID)
                .Single();

            // incrementing capacity to update database,
            // since user left all roles connected to the event
            role.number_limit += 1;

            // update in database
            await DatabaseConnector.Client
                .From<Models.EventRole>()
                .Where(v => v.event_ID == data.event_ID)
                .Where(v => v.role_ID == ev.role_ID)
                .Set(v => v.number_limit, role.number_limit)
                .Update();
        }

        await DatabaseConnector.Client
           .From<Models.VolunteerEvent>()
           .Where(v => v.volunteer_ID == response.volunteer_ID)
           .Where(v => v.event_ID == data.event_ID)
           .Delete();

        // recreate events on screen
        var currentPage = (ManagePage)Shell.Current.CurrentPage;
        currentPage.ClearEvents();
        currentPage.ShowEvents();
    }
}