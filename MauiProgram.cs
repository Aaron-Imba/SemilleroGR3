using Microsoft.Extensions.Logging;
using SemilleroGR3.Services;
using SemilleroGR3.ViewModels;
using SemilleroGR3.Views;
using CommunityToolkit.Maui; // Habilita el Toolkit correctamente

namespace SemilleroGR3
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            // 1. Registrar Servicios (Singletons porque solo necesitamos una instancia viva)
            builder.Services.AddSingleton<ApiClient>();
            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddSingleton<FamiliaService>();

            // 2. Registrar ViewModels (Transient crea una nueva instancia cada vez que se requiere)
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<DashboardViewModel>();
            builder.Services.AddTransient<PerfilHijoViewModel>();
            builder.Services.AddTransient<SeguimientoViewModel>();
            builder.Services.AddTransient<HistorialViewModel>();
            builder.Services.AddTransient<TareasCasaViewModel>();

            // 3. Registrar Views (Transient para que recarguen UI si es necesario)
            builder.Services.AddTransient<LoginView>();
            builder.Services.AddTransient<DashboardView>();
            builder.Services.AddTransient<PerfilHijoView>();
            builder.Services.AddTransient<SeguimientoView>();
            builder.Services.AddTransient<HistorialView>();
            builder.Services.AddTransient<TareasCasaView>();

            return builder.Build();
        }
    }
}