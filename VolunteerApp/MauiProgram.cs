using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using VolunteerApp.Content.Pages.Login;
using VolunteerApp.Services;

namespace VolunteerApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Tagesschrift-Regular.ttf", "Tagesschrift");
                });
            builder.Services.AddTransient<AccountPage>();
            builder.UseMauiCommunityToolkit();
#if DEBUG
            builder.Logging.AddDebug();
#endif            
            return builder.Build();
        }
    }
}
