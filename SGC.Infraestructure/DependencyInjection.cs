using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGC.Domain.Interfaces;
using SGC.Domain.Interfaces.ILogger;
using SGC.Infraestructure.Cache;
using SGC.Infraestructure.Email;
using SGC.Infraestructure.IA;
using SGC.Infraestructure.Logging;
using SGC.Infraestructure.Pagos;
using SGC.Infraestructure.SignalR.Hubs;
using SGC.Infraestructure.SignalR.Services;
using SGC.Infraestructure.SMS;
using StackExchange.Redis;

namespace SGC.Infraestructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration config)
        {
            // Email
            services.AddScoped<IEmailService, EmailService>();

            // SMS
            services.AddScoped<ISmsService, SmsService>();

            // Logger
            services.AddScoped<ISGCLogger, SGCLogger>();

            // Stripe
            services.AddScoped<IPaymentService, StripePaymentService>();
            services.AddScoped<StripeWebhookService>();

            // AI Chatbot
            services.AddHttpClient<IChatbotService, AnthropicChatService>();

            // Redis
            var redisConnection = config.GetConnectionString("Redis");
            if (string.IsNullOrWhiteSpace(redisConnection))
                throw new InvalidOperationException("ConnectionStrings:Redis no configurado");

            services.AddSingleton<IConnectionMultiplexer>(_ =>
                ConnectionMultiplexer.Connect(
                    redisConnection));
            services.AddScoped<ICacheService, RedisCacheService>();

            // SignalR
            services.AddSignalR();
            services.AddScoped<SignalRNotificacionService>();

            return services;
        }
    }
}
