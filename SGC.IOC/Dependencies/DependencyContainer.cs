using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGC.Application.Contracts;
using SGC.Application.Services;
using SGC.Domain.Interfaces.Repository;
using SGC.Persistence.Context;
using SGC.Persistence.Repositories.Appointments;
using SGC.Persistence.Repositories.Audit;
using SGC.Persistence.Repositories.Catalog;
using SGC.Persistence.Repositories.Medical;
using SGC.Persistence.Repositories.Notifications;
using SGC.Persistence.Repositories.Security;

namespace SGC.IOC.Dependencies
{
    // Contenedor central de dependencias para registrar servicios y repositorios.
    public static class DependencyContainer
    {
        // Registra el DbContext, repositorios y servicios de la aplicacion.
        public static IServiceCollection AddSGCDependencies(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Configura el DbContext con PostgreSQL usando el connection string.
            services.AddDbContext<SGCDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("SGCConnection")));

            // Repositorios de persistencia por interfaz.
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

            // Servicios de la capa Application.
            services.AddScoped<CitaDomainService>();
            services.AddScoped<ICitaService, CitaService>();
            services.AddScoped<IDisponibilidadService, DisponibilidadService>();
            services.AddScoped<IEspecialidadService, EspecialidadService>();
            services.AddScoped<IMedicoService, MedicoService>();
            services.AddScoped<INotificacionService, NotificacionService>();
            services.AddScoped<IPacienteService, PacienteService>();
            services.AddScoped<IPrefNotificacionService, PrefNotificacionService>();
            services.AddScoped<IProveedorSaludService, ProveedorSaludService>();
            services.AddScoped<IUsuarioService, UsuarioService>();

            return services;
        }
    }
}
