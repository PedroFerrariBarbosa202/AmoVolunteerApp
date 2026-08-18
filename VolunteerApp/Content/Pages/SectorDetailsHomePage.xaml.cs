using VolunteerApp.Cards;
using VolunteerApp.Services;
using VolunteerApp.Content.Forms;

namespace VolunteerApp.Pages;

public partial class SectorDetailsHomePage : ContentPage
{
	public SectorDetailsHomePage()
	{
		InitializeComponent();
		InitSectorDetailCards();
	}

	private async void InitSectorDetailCards() {
        // add loading page
        OverlayManager.SetLoadingOverlay(ContentGrid);

        // convert query to command
        var response = await DatabaseConnector.Client
            .From<Models.Sector>()
            .Get();

        List<Models.Sector> events = response.Models;

        foreach (Models.Sector row in events) {
            SectorDetailCard card = new SectorDetailCard(row);
            SectorDetailsCardStack.Children.Add(card);
        }

        OverlayManager.RemoveLoadingOverlay(ContentGrid);
    }

    public void InstantiateSectorDetailView(Models.Sector sector_data) {
        var view = new SectorDetailView(sector_data);
        ContentGrid.Children.Add(view);
    }
}