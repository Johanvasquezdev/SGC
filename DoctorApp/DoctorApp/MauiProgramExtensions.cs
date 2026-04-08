using Microsoft.Extensions.Logging;
using FluentValidation;
using DoctorApp.Views;
using DoctorApp.ViewModels;
using DoctorApp.Converters;
using DoctorApp.Services.ApiClient;
using DoctorApp.Services.Interfaces;
using DoctorApp.Services.Implementation;
using DoctorApp.Services.Mock;
using DoctorApp.Services.Hubs;
using DoctorApp.Security;
using DoctorApp.Validators;

namespace DoctorApp
{
    public static class MauiProgramExtensions
    {
        // ✅ Configuración de la API - URL BASE actualizada
        private const string API_BASE_URL = "http://localhost:5189/api";

        public static MauiAppBuilder UseSharedMauiApp(this MauiAppBuilder builder)
        {
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .RegisterApiServices()
                .RegisterValidators()
                .RegisterViews()
                .RegisterViewModels();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder;
        }

        /// <summary>
        /// ✅ Registra HttpClient y servicios de API REALES (no mocks)
        /// Usa ApiClient para consumir datos desde http://localhost:5189/api
        /// </summary>
        private static MauiAppBuilder RegisterApiServices(this MauiAppBuilder builder)
        {
            // ✅ Token Manager para autenticación JWT
            var tokenManager = new TokenManager();
            builder.Services.AddSingleton<ITokenManager>(tokenManager);

            // ✅ Configurar HttpClientHandler base
            var httpClientHandler = new HttpClientHandler();

            // ✅ Agregar DelegatingHandler para inyectar token JWT automáticamente
            var authHandler = new AuthenticationDelegatingHandler(tokenManager)
            {
                InnerHandler = httpClientHandler
            };

            // ✅ Crear HttpClient con URL base configurada
            var httpClient = new HttpClient(authHandler)
            {
                BaseAddress = new Uri(API_BASE_URL),
                Timeout = TimeSpan.FromSeconds(30)
            };

            System.Diagnostics.Debug.WriteLine($"[MauiProgramExtensions] HttpClient configurado con URL base: {API_BASE_URL}");

            // ✅ Registrar HttpClient como Singleton
            builder.Services.AddSingleton(httpClient);

            // ✅ Registrar IApiClient con implementación REAL (ApiClient, no Mock)
            builder.Services.AddSingleton<IApiClient, ApiClient>();

            // ✅ Registrar servicios reales (consumen datos de la API)
            builder.Services
                .AddScoped<ICitasService, CitasService>()
                .AddScoped<IDisponibilidadService, DisponibilidadService>()
                .AddScoped<IAuthService, AuthService>()
                .AddScoped<IDoctorService, DoctorService>()
                .AddScoped<ICitasHubClient, CitasHubClient>()
                .AddScoped<IDisponibilidadHubClient, DisponibilidadHubClient>();

            System.Diagnostics.Debug.WriteLine("[MauiProgramExtensions] ✅ Servicios reales registrados (ApiClient, no Mock)");

            return builder;
        }

        /// <summary>
        /// Registra los validadores de FluentValidation
        /// </summary>
        private static MauiAppBuilder RegisterValidators(this MauiAppBuilder builder)
        {
            builder.Services
                .AddSingleton<ConfirmarCitaValidator>()
                .AddSingleton<IniciarConsultaValidator>()
                .AddSingleton<MarcarAsistenciaValidator>()
                .AddSingleton<CrearDisponibilidadValidator>()
                .AddSingleton<ActualizarDisponibilidadValidator>()
                .AddSingleton<RegistrarDoctorValidator>()
                .AddSingleton<DoctorLoginValidator>();

            return builder;
        }

        private static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
        {
            builder.Services
                .AddSingleton<DashboardPage>()
                .AddSingleton<GestionDisponibilidadPage>()
                .AddSingleton<GestionCitasPage>()
                .AddSingleton<PanelConsultasDelDiaPage>();

            return builder;
        }

        private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
        {
            builder.Services
                .AddSingleton<DashboardViewModel>()
                .AddSingleton<GestionDisponibilidadViewModel>()
                .AddSingleton<GestionCitasViewModel>();

            return builder;
        }
    }
}
