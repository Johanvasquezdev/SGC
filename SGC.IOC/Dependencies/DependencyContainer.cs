using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// Repositorios - Interfaces
using SGC.Domain.Interfaces.Repository;
using SGC.Domain.Interfaces;
using SGC.Domain.Interfaces.ILogger;

// Repositorios - Implementaciones
using SGC.Persistence.Context;
using SGC.Persistence.Repositories.Appointments;
using SGC.Persistence.Repositories.Audit;
using SGC.Persistence.Repositories.Catalog;
using SGC.Persistence.Repositories.Medical;
using SGC.Persistence.Repositories.Notifications;
using SGC.Persistence.Repositories.Payments;
using SGC.Persistence.Repositories.Security;

// Servicios - Contratos
using SGC.Application.Contracts;

// Servicios - Implementaciones
using SGC.Application.Services;

// Domain Services y Validators
using SGC.Domain.Services;
using SGC.Domain.Validators;

// Infrastructure (registro centralizado)
using SGC.Infrastructure;

namespace SGC.IOC
{
    // Contenedor central de inyección de dependencias del sistema
    public static class DependencyContainer
    {
        // Registra todos los servicios, repositorios y el DbContext
        public static IServiceCollection AddSGCDependencies(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Fix para timestamps sin timezone con Npgsql y PostgreSQL
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            // ============================================================
            // 1. DbContext — PostgreSQL via Supabase
            // ============================================================
            services.AddDbContext<SGCDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("SGCConnection")));

            // ============================================================
            // 2. Repositorios
            // ============================================================
            services.AddScoped<ICitaRepository, CitaRepository>();
            services.AddScoped<IDisponibilidadRepository, DisponibilidadRepository>();
            services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();
            services.AddScoped<IEspecialidadRepository, EspecialidadRepository>();
            services.AddScoped<IProveedorSaludRepository, ProveedorSaludRepository>();
            services.AddScoped<IMedicoRepository, MedicoRepository>();
            services.AddScoped<IPacienteRepository, PacienteRepository>();
            services.AddScoped<INotificacionRepository, NotificacionRepository>();
            services.AddScoped<IPrefNotificacionRepository, PrefNotificacionRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IPagoRepository, PagoRepository>();

            // ============================================================
            // 3. Validators
            // ============================================================
            services.AddScoped<CitaValidator>();
            services.AddScoped<MedicoValidator>();
            services.AddScoped<PacienteValidator>();
            services.AddScoped<EspecialidadValidator>();
            services.AddScoped<DisponibilidadValidator>();
            services.AddScoped<ProveedorSaludValidator>();
            services.AddScoped<PrefNotificacionValidator>();
            services.AddScoped<UsuarioValidator>();

            // ============================================================
            // 4. Domain Services
            // ============================================================
            services.AddScoped<CitaDomainService>();

            // ============================================================
            // 5. Infrastructure
            // ============================================================
            services.AddInfrastructure(configuration);

            // ============================================================
            // 6. Application Services
            // ============================================================
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICitaService, CitaService>();
            services.AddScoped<IDisponibilidadService, DisponibilidadService>();
            services.AddScoped<IEspecialidadService, EspecialidadService>();
            services.AddScoped<IMedicoService, MedicoService>();
            services.AddScoped<INotificacionService, NotificacionService>();
            services.AddScoped<IPacienteService, PacienteService>();
            services.AddScoped<IPrefNotificacionService, PrefNotificacionService>();
            services.AddScoped<IProveedorSaludService, ProveedorSaludService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IPagoService, PagoService>();
            services.AddScoped<IChatbotAppService, ChatbotAppService>();
            services.AddScoped<IAuditoriaService, AuditoriaService>();

            return services;
        }
    }
}
