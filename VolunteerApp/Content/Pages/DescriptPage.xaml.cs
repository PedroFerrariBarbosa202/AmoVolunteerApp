using VolunteerApp.Services;
using VolunteerApp.Content.Forms;

namespace VolunteerApp.Pages
{
    public partial class DescriptPage : ContentPage
    {
        Models.Events _data;
        public DescriptPage(Models.Events data)
        {
            InitializeComponent();

            _data = data;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SetDescriptions();
        }

        private void SetDescriptions()
        {
            Title.Text = _data.name;
            Description.Text = _data.description;
            DateTime.Text = _data.date.ToString();
            Link.Text = _data.link;
        }

        private async void OpenRoleSetter(object sender, EventArgs e) {
            if (!UserService.UService.IsLoggedIn()) {
                await DisplayAlert(
                    "Usuário desconhecido",
                    "Crie uma conta para prosseguir",
                    "Continuar");
                return;
            }


            RoleSetter role_setter = new RoleSetter(_data);
            ContentStack.Add(role_setter);
        }

        private async void OnLinkTapped(object sender, EventArgs e) {
            try {
                await Launcher.Default.OpenAsync(_data.link);
            }
            catch (Exception) {
                await DisplayAlert(
                    "Link inválido",
                    "O link oferecido aparenta não ser um link válido.",
                    "Continuar");
            }
        }

    }
}
