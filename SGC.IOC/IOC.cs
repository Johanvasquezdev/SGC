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
using System;

namespace SGC.IOC
{
    // Registro central de dependencias para las capas Application y Persistence.
    public static class IOC
    {
        // Agrega DbContext, repositorios y servicios de aplicacion al contenedor DI.
        public static IServiceCollection AddSGCDependencies(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Obtiene el connection string principal desde la configuracion.
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' not found.");

            // Registra el DbContext con el proveedor de PostgreSQL.
            services.AddDbContext<SGCDbContext>(options =>
                options.UseNpgsql(connectionString));

            // Repositorios de persistencia.
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();
            services.AddScoped<IMedicoRepository, MedicoRepository>();
            services.AddScoped<IPacienteRepository, PacienteRepository>();
            services.AddScoped<IEspecialidadRepository, EspecialidadRepository>();
            services.AddScoped<IProveedorSaludRepository, ProveedorSaludRepository>();
            services.AddScoped<ICitaRepository, CitaRepository>();
            services.AddScoped<IDisponibilidadRepository, DisponibilidadRepository>();
            services.AddScoped<INotificacionRepository, NotificacionRepository>();
            services.AddScoped<IPrefNotificacionRepository, PrefNotificacionRepository>();

            // Servicios de la capa Application.
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IMedicoService, MedicoService>();
            services.AddScoped<IPacienteService, PacienteService>();
            services.AddScoped<IEspecialidadService, EspecialidadService>();
            services.AddScoped<IProveedorSaludService, ProveedorSaludService>();
            services.AddScoped<ICitaService, CitaService>();
            services.AddScoped<IDisponibilidadService, DisponibilidadService>();
            services.AddScoped<INotificacionService, NotificacionService>();
            services.AddScoped<IPrefNotificacionService, PrefNotificacionService>();

            // Servicio de dominio usado por la capa Application.
            services.AddScoped<CitaDomainService>();

            return services;
        }
    }
}
