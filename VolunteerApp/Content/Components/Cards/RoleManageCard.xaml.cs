using Microsoft.Data.SqlClient;
using Microsoft.Maui.Storage;
using System.Data;
using VolunteerApp.Services;
namespace VolunteerApp.Cards;

public partial class RoleManageCard : ContentView {
    Models.EventRole role_data;

    public RoleManageCard(Models.EventRole _role_data) {
        InitializeComponent();
        role_data = _role_data;
        BindingContext = _role_data;
        LimitLabel.Text = $"Número limite: {role_data.number_limit}";

    }
    public string RoleName => RoleNameLabel.Text;

    private async void OnCloseClicked(object sender, EventArgs e) {
        await DatabaseConnector.InitializeAsync();

        await DatabaseConnector.Client
            .From<Models.VolunteerEvent>()
            .Where(v => v.role_ID == role_data.role_ID)
            .Delete();

        await DatabaseConnector.Client
            .From<Models.EventRole>()
            .Where(v => v.role_ID == role_data.role_ID)
            .Delete();

        await DatabaseConnector.Client
            .From<Models.Roles>()
            .Where(v => v.role_ID == role_data.role_ID)
            .Delete();

        if (this.Parent is Layout parentLayout) {
            parentLayout.Children.Remove(this);
        }
    }
}