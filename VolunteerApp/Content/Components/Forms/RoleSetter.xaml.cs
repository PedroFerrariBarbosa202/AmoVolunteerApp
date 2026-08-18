using VolunteerApp.Services;
using VolunteerApp.Cards;
using System.Diagnostics;

namespace VolunteerApp.Content.Forms;

public partial class RoleSetter : ContentView {
    Models.Events ev_data;

    public RoleSetter(Models.Events _ev_data) {
        ev_data = _ev_data;

        InitializeComponent();
        InitRolePicker();
        InitDatePicker();
    }

    private async void InitRolePicker() {
        // clear picker before init
        RolePicker.Items.Clear();

        var ev_roles = await DatabaseConnector.Client
            .From<Models.EventRole>()
            .Where(v => v.event_ID == ev_data.event_ID)
            .Where(v => v.number_limit > 0)
            .Get();

        foreach (Models.EventRole evr in ev_roles.Models) {
            var role = await DatabaseConnector.Client
                .From<Models.Roles>()
                .Where(v => v.role_ID == evr.role_ID)
                .Single();

            if (role == null)
                continue;

            RolePicker.Items.Add(role.name);         
        }

        // Add "No items" item if no role is connected
        if (RolePicker.Items.Count <= 0) {
            RolePicker.Title = "Nenhum papél disponível";
            RolePicker.IsEnabled = false;
        }
    }

    private async void InitDatePicker() {
        // clear picker before init
        DataPicker.Items.Clear();

        // gets the date from the event selecteed
        await DatabaseConnector.InitializeAsync();
        var response = await DatabaseConnector.Client
            .From<Models.Events>()
            .Where(v => v.event_ID == ev_data.event_ID)
            .Get();

        List<Models.Events> events = response.Models;

        DateOnly? _date = events[0].date;

        if (_date == null)
            return;

        DataPicker.Items.Add(_date.ToString());
    }

    private async void OnPickerChanged(object sender, EventArgs e) {
        // get role_id by name
        var role_item = RolePicker.Items[RolePicker.SelectedIndex];
        var role = await DatabaseConnector.Client
            .From<Models.Roles>()
            .Where(v => v.name == role_item)
            .Single();

        // get roles that are connected to event, taking out the ones with no space left
        var response2 = await DatabaseConnector.Client
            .From<Models.EventRole>()
            .Where(v => v.event_ID == ev_data.event_ID)
            .Where(v => v.role_ID == role.role_ID)
            .Get();

        List<Models.EventRole> evr_numlimit = response2.Models;

        Models.EventRole _r_data = new Models.EventRole {
            role_ID = role.role_ID,
            event_ID = ev_data.event_ID,
            name = RolePicker.Items[RolePicker.SelectedIndex],
            number_limit = evr_numlimit[0].number_limit,
        };
        var roleManageCard = new RoleManageCard(_r_data);
        RoleStack.Children.Add(roleManageCard);

        // enable the button so user can end subscription
        SetFinalizationButton(true);
    }

    private async void AddUser(object sender, EventArgs e) {
        Models.Volunteer userData = AccountState.volunteerData;

        if (userData != null) {
            // get role_id by name
            await DatabaseConnector.InitializeAsync();
            var response = await DatabaseConnector.Client
                .From<Models.Volunteer>()
                .Where(v => v.email == userData.email)
                .Get();

            List<Models.Volunteer> volunteer = response.Models;

            if (volunteer == null) {
                Debug.WriteLine("User not found in database!");
                return;
            }

            foreach (var child in RoleStack.Children) {
                if (child is RoleManageCard roleCard) {
                    // getting role_id
                    await DatabaseConnector.InitializeAsync();
                    var role_response = await DatabaseConnector.Client
                        .From<Models.Roles>()
                        .Where(v => v.name == roleCard.RoleName)
                        .Get();

                    List<Models.Roles> roles = role_response.Models;

                    // add user
                    // create volunteer_event connection model to add to database
                    var vol_model = new Models.VolunteerEvent {
                        event_ID = ev_data.event_ID,
                        volunteer_ID = volunteer[0].volunteer_ID,
                        role_ID = roles[0].role_ID,
                        date = DateOnly.Parse(DataPicker.Items[0])
                    };

                    await DatabaseConnector.Client.From<Models.VolunteerEvent>().Insert(vol_model);

                    // reduce number of volunteers in role by 1
                    // Read the current value
                    int role_ID = roles[0].role_ID;
                    var currentRole = await DatabaseConnector.Client
                        .From<Models.EventRole>()
                        .Where(v => v.event_ID == ev_data.event_ID)
                        .Where(v => v.role_ID == role_ID)
                        .Single();

                    // Subtract 1
                    var newLimit = currentRole.number_limit - 1;

                    // Update
                    await DatabaseConnector.Client
                        .From<Models.EventRole>()
                        .Where(v => v.event_ID == ev_data.event_ID)
                        .Where(v => v.role_ID == role_ID)
                        .Set(x => x.number_limit, newLimit)
                        .Update();
                }
            }

            // debugs
            await Application.Current.MainPage.DisplayAlert("Status de inscrição", "Inscrição efetuada com sucesso!", "OK");
            Debug.WriteLine("USER CONNECTED TO EVENT!");

            // close
            if (this.Parent is Layout parentLayout) {
                parentLayout.Children.Remove(this);
            }
        }
    }

    private void OnCloseClicked(object sender, EventArgs e) {
        if (this.Parent is Layout parentLayout) {
            parentLayout.Children.Remove(this);
        }
    }

    private void SetFinalizationButton(bool val) {
        FinalizationButton.IsEnabled = val;

        switch (val) {
            case true:
                FinalizationButton.BackgroundColor = Color.FromArgb("#4CAF50");
                break;
            case false:
                FinalizationButton.BackgroundColor = Colors.Gray;
                break;
        }
    }
}