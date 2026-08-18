namespace VolunteerApp.Content.Forms;

public partial class SectorDetailView : ContentView
{
	public SectorDetailView(Models.Sector sector_data)
	{
		InitializeComponent();
	}

    private async void OnWatchClicked(object sender, EventArgs e) {
        await Launcher.Default.OpenAsync(
            "https://www.youtube.com/watch?v=ZrkMueJ65Zc");
    }

    private void OnCloseClicked(object sender, EventArgs e) {
        if (this.Parent is Layout parentLayout) {
            parentLayout.Children.Remove(this);
        }
    }
}