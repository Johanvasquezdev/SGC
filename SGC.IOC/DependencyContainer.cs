using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// Repositorios - Interfaces (Domain)
using SGC.Domain.Interfaces.Repository;

// Repositorios - Implementaciones (Persistence)
using SGC.Persistence.Context;
using SGC.Persistence.Repositories.Appointments;
using SGC.Persistence.Repositories.Audit;
using SGC.Persistence.Repositories.Catalog;
using SGC.Persistence.Repositories.Medical;
using SGC.Persistence.Repositories.Notifications;
using SGC.Persistence.Repositories.Security;

// Servicios - Interfaces (Application Contracts)
using SGC.Application.Contracts;

// Servicios - Implementaciones (Application Services)
using SGC.Application.Services;

namespace SGC.IOC
{
    // Contenedor de inyeccion de dependencias.
    // Registra todos los servicios, repositorios y el DbContext del sistema.
    public static class DependencyContainer
    {
        // Metodo de extension para registrar todas las dependencias del sistema en el contenedor de servicios
        public static IServiceCollection AddSGCDependencies(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // ============================================================
            // 1. Registro del DbContext con PostgreSQL
            // ============================================================
            services.AddDbContext<SGCDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("SGCConnection")));

            // ============================================================
            // 2. Registro de repositorios (Persistence -> Domain interfaces)
            // ============================================================

            // Appointments
            services.AddScoped<ICitaRepository, CitaRepository>();
            services.AddScoped<IDisponibilidadRepository, DisponibilidadRepository>();

            // Audit
            services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();

            // Catalog
            services.AddScoped<IEspecialidadRepository, EspecialidadRepository>();
            services.AddScoped<IProveedorSaludRepository, ProveedorSaludRepository>();

            // Medical
            services.AddScoped<IMedicoRepository, MedicoRepository>();
            services.AddScoped<IPacienteRepository, PacienteRepository>();

            // Notifications
            services.AddScoped<INotificacionRepository, NotificacionRepository>();
            services.AddScoped<IPrefNotificacionRepository, PrefNotificacionRepository>();

            // Security
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            // ============================================================
            // 3. Registro de servicios de aplicacion (Application layer)
            // ============================================================

            // Servicio de logica de negocio para validaciones de citas
            services.AddScoped<CitaDomainService>();

            // Servicios de aplicacion
            services.AddScoped<ICitaService, CitaService>();
            services.AddScoped<IDisponibilidadService, DisponibilidadService>();
            services.AddScoped<IEspecialidadService, EspecialidadService>();
            services.AddScoped<IMedicoService, MedicoService>();
            services.AddScoped<INotificacionService, NotificacionService>();
            services.AddScoped<IPacienteService, PacienteService>();
            services.AddScoped<IPrefNotificacionService, PrefNotificacionService>();
            services.AddScoped<IProveedorSaludService, ProveedorSaludService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            // Servicios adicionales de autenticacion, auditoria, pagos y chatbot.
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuditoriaService, AuditoriaService>();
            services.AddScoped<IChatbotAppService, ChatbotAppService>();
            services.AddScoped<IPagoService, PagoService>();

            return services;
        }
    }
}
