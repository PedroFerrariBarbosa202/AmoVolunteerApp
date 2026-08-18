namespace VolunteerApp.Content.Pages.Login;

public partial class ContactPage : ContentPage
{
	public ContactPage()
	{
		InitializeComponent();

        // return to home screen
        ToolbarItems.Add(new ToolbarItem {
            Text = "Voltar",
            Command = new Command(async () =>
                await Shell.Current.GoToAsync("//MainPage"))
        });
    }
}