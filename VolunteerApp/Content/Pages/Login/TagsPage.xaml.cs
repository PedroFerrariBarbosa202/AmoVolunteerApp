using Azure;
using VolunteerApp.Content.Tags;
using VolunteerApp.Services;

namespace VolunteerApp.Content.Pages.Login;

public partial class TagsPage : ContentPage
{
    public List<Models.Sector> TagsConnected = new();
    public List<Models.Sector> TagsToChoose = new();

    public TagsPage()
	{
		InitializeComponent();
        InitializeTags();

        // return to home screen
        ToolbarItems.Add(new ToolbarItem {
            Text = "Voltar",
            Command = new Command(async () =>
                await Shell.Current.GoToAsync("//MainPage"))
        });
    }

	private async void InitializeTags() {
        // get data from database
        await DatabaseConnector.InitializeAsync();
        var response = await DatabaseConnector.Client
                .From<Models.Sector>()
                .Get();

        foreach (var sector in response.Models) {
            var tag_obj = new Tag(sector);

            TagsToChoose.Add(sector);
            ToChooseSectors.Children.Add(tag_obj);
        }
    }

    public async void RefreshTags() {
        // clear stacks
        ConnectedSectors.Children.Clear();
        ToChooseSectors.Children.Clear();

        await DatabaseConnector.InitializeAsync();
        var response = await DatabaseConnector.Client
                .From<Models.Sector>()
                .Get();

        foreach (var sector in response.Models) {
            var tag_obj = new Tag(sector);

            if (TagsConnected.Any(t => t.sector_ID == sector.sector_ID))
                ConnectedSectors.Children.Add(tag_obj);

            else
                ToChooseSectors.Children.Add(tag_obj);
        }
    }
}