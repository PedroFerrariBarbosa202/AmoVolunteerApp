using System.Text;
using VolunteerApp.Content.Pages.Login;
using VolunteerApp.Services;


namespace VolunteerApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            InitializeSideBar();
        }

        async void AccountRedirect(object sender, EventArgs e) {
            if (!AccountState.volunteerData.logged_in) {
                Shell.Current.FlyoutIsPresented = false;
                await Shell.Current.GoToAsync($"///{nameof(AccountLogPage)}");
                return;
            }

            // check if account if verified
            if (!AccountState.volunteerData.is_validated && AccountState.volunteerData.logged_in) {
                Shell.Current.FlyoutIsPresented = false;
                await Shell.Current.GoToAsync($"//ContactPage");
                return;
            }

            // close side bar
            Shell.Current.FlyoutIsPresented = false;
            await Shell.Current.GoToAsync($"///{nameof(AccountPage)}");
        }

        public async void InitializeSideBar() {
            await DatabaseConnector.InitializeAsync();
            Models.Volunteer user_data = AccountState.volunteerData;

            // change subtitle according to solicitation status
            if (user_data == null) {
                SubTitle.Text = "";
            }

            // check if account is validated
            if (user_data != null) {
                var account = await DatabaseConnector.Client
                    .From<Models.Volunteer>()
                    .Where(v => v.email == AccountState.volunteerData.email)
                    .Where(v => v.password == AccountState.volunteerData.password)
                    .Single();

                // change subtitle according to solicitation status
                if (account != null) {
                    if (!account.is_validated) {
                        if (account.solicitation_seen == false) {
                            SubTitle.Text = "Status da conta: NÃO VISUALIZADA";
                            SubTitle.TextColor = Colors.Red;
                        }
                        else if (account.solicitation_seen == true) {
                            SubTitle.Text = "Status da conta: EM VERIFICAÇÃO";
                            SubTitle.TextColor = Colors.Green;
                        }
                    }
                    else {
                        SubTitle.Text = "Status da conta: CONTA VERIFICADA";
                        SubTitle.TextColor = Colors.White;
                    }
                }
            }
            if (user_data != null && user_data.user_img != null) {
                byte[] imageByte = Encoding.UTF8.GetBytes(user_data.user_img);
                ImageSource img = user_data != null && user_data.user_img.Count() > 0 ?
                    UserService.UserImgService.BytesToImageSource(imageByte) :
                    ImageSource.FromFile("no_img.png");
                UserIcon.Source = img;
            }

            UserName.Text = user_data != null ? user_data.name : "Nenhuma Conta";
        }
    }
}
