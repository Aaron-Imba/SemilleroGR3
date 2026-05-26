using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SemilleroGR3.ViewModels;
using SemilleroGR3.Views;

namespace SemilleroGR3
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            }).UseMauiCommunityToolkit();
            builder.Services.AddTransient<ProgresoViewModel>();
            builder.Services.AddTransient<ProgresoView>();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}