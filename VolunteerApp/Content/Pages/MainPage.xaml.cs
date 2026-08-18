using System.Diagnostics;
using VolunteerApp.Content.Pages.Login;
using VolunteerApp.Services;
using VolunteerApp.Cards;


namespace VolunteerApp.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage() {
            InitializeComponent();
        }

        protected override async void OnAppearing() {
            base.OnAppearing();
            await InitNewsCards(null, null);
        }

        private async void GoToSecondPage(object sender, EventArgs e){
            await Navigation.PushAsync(new SecondPage());
        }
        private async void GoToManagePage(object sender, EventArgs e)
        {
            if(AccountState.volunteerData == null) {
                await DisplayAlert(
                    "Usuário desconhecido",
                    "Crie uma conta para prosseguir",
                    "Continuar");
                return;
            }
            await Navigation.PushAsync(new ManagePage());
        }
        private async void GoToLoginPage(object sender, EventArgs e) {
            if (AccountState.volunteerData != null) {
                await Shell.Current.GoToAsync($"///{nameof(AccountPage)}");
            }
            else {
                await Shell.Current.GoToAsync($"///{nameof(AccountLogPage)}");
            }
        }
        private async Task InitNewsCards(object? sender, EventArgs? e) {
            OverlayManager.SetLoadingOverlay(ContentGrid);

            var response = await DatabaseConnector.Client
                 .From<Models.News>()
                 .Get();

            if(response.Models.Count == 0) {
                NoNewsLabel.Text = "Nenhuma notícia encontrada :(";
                OverlayManager.RemoveLoadingOverlay(ContentGrid);
                return;
            }

            foreach (var item in response.Models) {
                Models.News news = new Models.News {
                    id = item.id,
                    title = item.title,
                    content = item.content,
                    image = item.image,
                    created_at = item.created_at,
                };
                NewsCard card = new NewsCard(news);
                NewsStack.Children.Add(card);
            }

            OverlayManager.RemoveLoadingOverlay(ContentGrid);
        }
    }
}
