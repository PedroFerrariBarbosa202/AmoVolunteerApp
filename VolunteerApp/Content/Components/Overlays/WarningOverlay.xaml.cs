using VolunteerApp.Services;
namespace VolunteerApp.Content.Overlays;

public partial class WarningOverlay : ContentView
{
	public WarningOverlay(WarningOverlayData data)
	{
		InitializeComponent();
		BindingContext = data;
	}

    private void OnCloseClicked(object? sender, EventArgs? e) {
        if (this.Parent is Layout parentLayout) {
            parentLayout.Children.Remove(this);
        }
    }
}