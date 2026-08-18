using VolunteerApp.Content.Pages.Login;
using VolunteerApp.Services;

namespace VolunteerApp.Content.Tags;

public partial class Tag : ContentView
{
    Models.Sector tag_data;

    public Tag(Models.Sector _tag_data)
	{
		InitializeComponent();
        tag_data = _tag_data;

        NameTag.Text = _tag_data.name;
        Border.Stroke = Color.FromArgb(_tag_data.color);
    }
    public bool ButtonIsActive {
        get => IncludeButton.IsEnabled;
        set => IncludeButton.IsEnabled = value;
    }

    private void OnSectorSelected(object sender, EventArgs e) {
        var page = GetParentPage();

        if (page is TagsPage page_tags) {
            var connectedTag = page_tags.TagsConnected
                .FirstOrDefault(t => t.sector_ID == tag_data.sector_ID);

            if (connectedTag == null) {
                page_tags.TagsToChoose.RemoveAll(
                    t => t.sector_ID == tag_data.sector_ID
                );

                page_tags.TagsConnected.Add(tag_data);
            }
            else {
                page_tags.TagsConnected.Remove(connectedTag);

                page_tags.TagsToChoose.Add(connectedTag);
            }

            page_tags.RefreshTags();
        }

        // add to general state of current account creation
        AccountState.sectors.Add(tag_data);
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