using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGC.Domain.Interfaces;
using SGC.Domain.Interfaces.ILogger;
using SGC.Infrastructure.Cache;
using SGC.Infrastructure.Email;
using SGC.Infrastructure.IA;
using SGC.Infrastructure.Logging;
using SGC.Infrastructure.Pagos;
using SGC.Infrastructure.SignalR.Hubs;
using SGC.Infrastructure.SignalR.Services;
using SGC.Infrastructure.SMS;
using StackExchange.Redis;

namespace SGC.Infrastructure
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
            services.AddSingleton<IConnectionMultiplexer>(_ =>
                ConnectionMultiplexer.Connect(
                    config.GetConnectionString("Redis")
                    ?? "localhost:6379"));
            services.AddScoped<ICacheService, RedisCacheService>();

            // SignalR
            services.AddSignalR();
            services.AddScoped<SignalRNotificacionService>();

            return services;
        }
    }
}