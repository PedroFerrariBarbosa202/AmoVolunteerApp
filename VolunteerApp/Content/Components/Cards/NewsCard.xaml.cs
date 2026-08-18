using VolunteerApp.Models;
using VolunteerApp.Services;

namespace VolunteerApp.Cards;

public partial class NewsCard : ContentView {
    public NewsCard(News _news) {
        InitializeComponent();

        byte[] img_byte = Convert.FromBase64String(_news.image);
        var img = UserService.UserImgService.BytesToImageSource(img_byte);

        TitleEntry.Text = _news.title;
        ContentEntry.Text = _news.content;
        ImageView.Source = img;
    }
}