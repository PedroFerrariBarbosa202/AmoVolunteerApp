using Microsoft.Data.SqlClient;
using System.Globalization;
using VolunteerApp.Content.Pages.Login;
using VolunteerApp.Services;

namespace VolunteerApp.Content.Forms;

public partial class EditInfoForm : ContentView
{
    string field;

    public EditInfoForm(string _field)
	{       
		InitializeComponent();
        field = _field;
        InfoEntry.Placeholder = _field;
    }

    private async void OnEditClicked(object sender, EventArgs e) {
        // edits the current user in database adding new info selected by current user
        UpdateCredential();
        await Application.Current.MainPage.DisplayAlert("Status da edição", "Edição feita com sucesso!","OK");
        await Shell.Current.GoToAsync($"///{nameof(AccountPage)}");
    }

    private void OnCloseClicked(object sender, EventArgs e) {
        if (this.Parent is Layout parentLayout) {
            parentLayout.Children.Remove(this);
        }
    }

    async void UpdateCredential() {
        var c_user = AccountState.volunteerData;

        switch (field) {
            case "nome":
            var response_name = await DatabaseConnector.Client
                .From<Models.Volunteer>()
                .Where(v => v.email == c_user.email)
                .Where(v => v.password == c_user.password)
                .Set(x => x.name, InfoEntry.Text)
                .Update();

            c_user.name = InfoEntry.Text;
            break;

            case "idade (dia/mês/ano)":
            if (DateOnly.TryParseExact(
                        InfoEntry.Text.Trim(),
                        "dd/MM/yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateOnly date)) {
            var response_age = await DatabaseConnector.Client
                .From<Models.Volunteer>()
                .Where(v => v.email == c_user.email)
                .Where(v => v.password == c_user.password)
                .Set(x => x.age, InfoEntry.Text)
                .Update();

                c_user.age = date;
            }

            break;

            case "senha":
            var response_password = await DatabaseConnector.Client
                .From<Models.Volunteer>()
                .Where(v => v.email == c_user.email)
                .Where(v => v.password == c_user.password)
                .Set(x => x.password, InfoEntry.Text)
                .Update();

            c_user.password = InfoEntry.Text;
            break;

            case "email":
            var respose_email = await DatabaseConnector.Client
                .From<Models.Volunteer>()
                .Where(v => v.email == c_user.email)
                .Where(v => v.password == c_user.password)
                .Set(x => x.email, InfoEntry.Text)
                .Update();

            c_user.email = InfoEntry.Text;
            break;

            case "telefone":
            var respose_phone = await DatabaseConnector.Client
                .From<Models.Volunteer>()
                .Where(v => v.email == c_user.email)
                .Where(v => v.password == c_user.password)
                .Set(x => x.phone, InfoEntry.Text)
                .Update();

            c_user.phone = InfoEntry.Text;
            break;

            case "empresa":
            var respose_company = await DatabaseConnector.Client
                .From<Models.Volunteer>()
                .Where(v => v.email == c_user.email)
                .Where(v => v.password == c_user.password)
                .Set(x => x.company, InfoEntry.Text)
                .Update();

            c_user.company = InfoEntry.Text;
            break;

            case "emprego":
            var respose_profession = await DatabaseConnector.Client
                .From<Models.Volunteer>()
                .Where(v => v.email == c_user.email)
                .Where(v => v.password == c_user.password)
                .Set(x => x.profession, InfoEntry.Text)
                .Update();

            c_user.profession = InfoEntry.Text;
            break;
        }
    }
}