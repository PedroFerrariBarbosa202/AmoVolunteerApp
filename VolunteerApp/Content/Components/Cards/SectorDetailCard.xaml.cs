using VolunteerApp.Pages;

namespace VolunteerApp.Cards;

public partial class SectorDetailCard : ContentView {
    Models.Sector sector_data = new Models.Sector();

    public SectorDetailCard() {
        InitializeComponent();
    }

    public SectorDetailCard(Models.Sector _sector_data) : this() {
        sector_data = _sector_data;
        InitUI(sector_data);
    }

    private void InitUI(Models.Sector sector_data) {
        TitleLabel.Text = sector_data.name;
        DescriptionLabel.Text = sector_data.details;

        DividerLine.BackgroundColor = Color.FromArgb(sector_data.color);
    }

    private void OnEnterSectorDetailsClicked(object sender, EventArgs e) {
        var page = GetParentPage() as SectorDetailsHomePage;
        if (page == null)
            return;

        page.InstantiateSectorDetailView(sector_data);
    }

    private Page? GetParentPage() {
        Element parent = this;

        while (parent != null) {
            if (parent is Page page)
                return page;

            parent = parent.Parent;
        }

        return null;
    }
}