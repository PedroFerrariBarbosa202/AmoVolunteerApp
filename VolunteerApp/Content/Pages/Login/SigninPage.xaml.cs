using System.Diagnostics;
using System.Globalization;
using System.Text;
using VolunteerApp.Pages;
using VolunteerApp.Services;
using static System.Net.Mime.MediaTypeNames;
namespace VolunteerApp.Content.Pages.Login;

public partial class SigninPage : ContentPage {
    byte[] user_image;

    public SigninPage() {
        InitializeComponent();

        // return to home screen
        ToolbarItems.Add(new ToolbarItem {
            Text = "Voltar",
            Command = new Command(async () =>
                await Shell.Current.GoToAsync("//MainPage"))
        });
    }

    private async void OnPickImageClicked(object sender, EventArgs e) {
        try {
            var result = await FilePicker.PickAsync(new PickOptions {
                PickerTitle = "Selecione uma imagem",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null) {
                UserImage.Source = ImageSource.FromFile(result.FullPath);
                user_image = await UserService.UserImgService.FileResultToBytesAsync(result);
                AccountState.volunteerData.user_img = Encoding.UTF8.GetString(user_image);
            }
        } catch (Exception ex) {
            await DisplayAlert("Erro", $"Não foi possível carregar a imagem: {ex.Message}", "OK");
        }
    }
    async void OnTextChanged(object sender, EventArgs e) {
        Debug.WriteLine(
        AccountState.volunteerData == null
            ? "volunteerData É NULL"
            : "volunteerData EXISTE"
    );

        var entry = (Entry)sender;
        entry.Text = entry.Text.Replace(" ", "").Trim();

        switch (entry.ClassId) {
            case ("NameEntry"):
                AccountState.volunteerData.name = entry.Text;
                break;

            case ("AgeEntry"):
                if (DateOnly.TryParseExact(
                        entry.Text,
                        "dd/MM/yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateOnly date)) {
                AccountState.volunteerData.age = date;
                }
                else {
                    await DisplayAlert("Erro", "Data incluída não é válida. Use o formato dd/MM/yyyy.", "OK");
                    return;
                }
            break;

            case ("EmailEntry"):
                AccountState.volunteerData.email = entry.Text;
                break;

            case ("PasswordEntry"):
                AccountState.volunteerData.password = entry.Text;
                break;

            case ("PhoneEntry"):
                AccountState.volunteerData.phone = entry.Text;
                break;

            case ("ProfessionEntry"):
                AccountState.volunteerData.profession = entry.Text;
                break;

            case ("CompanyEntry"):
                AccountState.volunteerData.company = entry.Text;
                break;
        }
    }
}