using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using Serilog;
using SGC.Domain.Interfaces;
using SGC.Domain.Interfaces.ILogger;
using SGC.Infraestructure.Cache;
using SGC.Infraestructure.Email;
using SGC.Infraestructure.SMS;
using SGC.Infraestructure.Pagos;
using SGC.Infraestructure.IA;
using SGC.Infraestructure.Logging;

namespace SGC.Infraestructure
{
    public static class Infra
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Redis
            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(
                    configuration["Redis:ConnectionString"]!));
            services.AddScoped<ICacheService, RedisCacheServices>();

            // Email
            services.AddScoped<IEmailService, EmailService>();

            // SMS
            services.AddScoped<ISmsService, SmsService>();

            // Stripe
            services.AddScoped<IPaymentService, StripePaymentService>();

            // Claude / Anthropic
            services.AddScoped<IChatbotService, AnthropicChatService>();

            // Logging
            services.AddScoped<ISGCLogger, SGCLogger>();

            // SignalR
            services.AddSignalR();

            return services;
        }
    }
}