using System.Text;
using VolunteerApp.Content.Forms;
using VolunteerApp.Content.Tags;
using VolunteerApp.Services;
namespace VolunteerApp.Content.Pages.Login;

public partial class AccountPage : ContentPage {
    public AccountPage() {
        InitializeComponent();
        InitEditFunctions();
        InitializeAsync();

        ToolbarItems.Add(new ToolbarItem {
            Text = "Voltar",
            Command = new Command(async () =>
                await Shell.Current.GoToAsync("//MainPage"))
        });
    }

    private void InitEditFunctions() {
        NameButton.Clicked += (s, e) => EditInfo("nome");
        AgeButton.Clicked += (s, e) => EditInfo("idade (dia/mês/ano)");
        EmailButton.Clicked += (s, e) => EditInfo("email");
        PasswordButton.Clicked += (s, e) => EditInfo("senha");
        PhoneButton.Clicked += (s, e) => EditInfo("telefone");
        CompanyButton.Clicked += (s, e) => EditInfo("empresa");
        ProfessionButton.Clicked += (s, e) => EditInfo("emprego");
    }

    private void EditInfo(string data) {
        EditInfoForm edit_info = new EditInfoForm(data);
        ContentStack.Add(edit_info);
    }

    private async void InitializeAsync() {
        await DatabaseConnector.InitializeAsync();
        await InitFields();   
        await InitSectors();  
    }

    private async Task InitFields() {
        var _currentUser = AccountState.volunteerData;

        // gets the information from the current user
        if (_currentUser != null) {
            var volunteer = await DatabaseConnector.Client
                .From<Models.Volunteer>()
                .Where(v => v.email == AccountState.volunteerData.email)
                .Where(v => v.password == AccountState.volunteerData.password)
                .Single();

            // set up fields
            if (volunteer != null) {
                NameLabel.Text = $"Nome: {volunteer.name}";
                AgeLabel.Text = $"Idade: {volunteer.age.ToString()}";
                EmailLabel.Text = $"Email: {volunteer.email}";
                PasswordLabel.Text = $"Senha: {volunteer.password}";
                PhoneLabel.Text = $"Telefone: {volunteer.phone}";
                ProfessionLabel.Text = $"Emprego: {volunteer.profession}";
                CompanyLabel.Text = $"Empresa: {volunteer.company}";

                if (!string.IsNullOrWhiteSpace(volunteer.user_img)) {
                    byte[] buffer = Convert.FromBase64String(volunteer.user_img);
                    ImageSource user_img = UserService.UserImgService.BytesToImageSource(buffer);

                    // Set img
                    UserImage.Source = user_img;
                }
            }
            else {
                UserService.UService.LogoutUser();
            }
        }
    }

    private async void OnPickImageClicked(object sender, EventArgs e) {
        try {
            var result = await FilePicker.PickAsync(new PickOptions {
                PickerTitle = "Selecione uma imagem",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null) {
                var _currentUser = AccountState.volunteerData;
                byte[] user_img = await UserService.UserImgService.FileResultToBytesAsync(result);

                // update user image on database
                await DatabaseConnector.Client
                    .From<Models.Volunteer>()
                    .Where(v => v.email == AccountState.volunteerData.email)
                    .Where(v => v.password == AccountState.volunteerData.password)
                    .Set(x => x.user_img, Convert.ToBase64String(user_img))
                    .Update();

                // update user data
                AccountState.volunteerData.user_img = Encoding.UTF8.GetString(user_img);
                UserImage.Source = ImageSource.FromFile(result.FullPath);
            }
        }
        catch (Exception ex) {
            await DisplayAlert("Erro", $"Não foi possível carregar a imagem: {ex.Message}", "OK");
        }
    }
    private async Task InitSectors() {
        var _currentUser = AccountState.volunteerData;

        var volunteer = await DatabaseConnector.Client
                .From<Models.Volunteer>()
                .Where(v => v.email == AccountState.volunteerData.email)
                .Where(v => v.password == AccountState.volunteerData.password)
                .Single();

        var response = await DatabaseConnector.Client
              .From<Models.VolunteerSector>()
              .Where(v => v.volunteer_ID == volunteer.volunteer_ID)
              .Where(v => v.is_validated == true)
              .Get();

        foreach(var connection in response.Models) {
            // get sector by id
            var sector_response = await DatabaseConnector.Client
              .From<Models.Sector>()
              .Where(v => v.sector_ID == connection.sector_ID)
              .Single();

            var sec_data = new Models.Sector {
                sector_ID = sector_response.sector_ID,
                name = sector_response.name,
                color = sector_response.color,
            };

            Tag tag = new Tag(sec_data);
            tag.ButtonIsActive = false;
            SectorStack.Children.Add(tag);
        }
    }


    public async void LogoutUser(object sender, EventArgs e) {
        UserService.UService.LogoutUser();
        AccountState.volunteerData = AccountState.volunteerDefault;
        await Application.Current.MainPage.DisplayAlert("Status de logout", "Conta desconectada com sucesso!", "OK");

        // voltar para MainPage
        await Shell.Current.GoToAsync("//MainPage");
    }
}