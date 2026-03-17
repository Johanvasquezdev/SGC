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
// Infrastructure
using SGC.Infrastructure.Email;
using SGC.Infrastructure.SMS;
using SGC.Infrastructure.Logging;
using SGC.Infrastructure.Payments;
using SGC.Infrastructure.AI;
using SGC.Infrastructure.Cache;

namespace SGC.IOC
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddSGCDependencies(
            this IServiceCollection services,
            IConfiguration configuration)
        {
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

            // ============================================================
            // 4. Domain Services
            // ============================================================
            services.AddScoped<CitaDomainService>();

            // ============================================================
            // 5. Application Services
            // ============================================================
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

            // ============================================================
            // 6. Infrastructure
            // ============================================================
            services.AddScoped<ISGCLogger, SGCLogger>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<IPaymentService, StripePaymentService>();
            services.AddHttpClient<IChatbotService, AnthropicChatService>();

            // ============================================================
            // 7. Redis Cache
            // ============================================================
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration
                    .GetConnectionString("Redis");
            });
            services.AddScoped<ICacheService, RedisCacheService>();

            // ============================================================
            // 8. SignalR
            // ============================================================
            services.AddSignalR();

            return services;
        }
    }
}
