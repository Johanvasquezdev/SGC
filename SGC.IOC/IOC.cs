using Microsoft.Extensions.DependencyInjection;
using SGC.Domain.Repository;
using SGC.Domain.Repository.Appointments;
using SGC.Domain.Repository.Medical;
using SGC.Domain.Repository.Notifications;
using SGC.Domain.Repository.Security;
using SGC.Persistence.Repositories.Appointments;
using SGC.Persistence.Repositories.Medical;
using SGC.Persistence.Repositories.Notifications;
using SGC.Persistence.Repositories.Security;

namespace SGC.IOC
{
    public static class IOC
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IMedicoRepository, MedicoRepository>();
            services.AddScoped<IPacienteRepository, PacienteRepository>();
            services.AddScoped<ICitaRepository, CitaRepository>();
            services.AddScoped<IDisponibilidadRepository, DisponibilidadRepository>();
            services.AddScoped<INotificacionRepository, NotificacionRepository>();
            services.AddScoped<IPrefNotificacionRepository, PrefNotificacionRepository>();

            return services;
        }
    }
}

