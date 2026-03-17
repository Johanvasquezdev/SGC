using Microsoft.Extensions.Configuration;
using Stripe;

namespace SGC.Infrastructure.Pagos
{
    // Servicio para validar los webhooks de Stripe y procesar eventos relacionados con los pagos de citas médicas
    public class StripeWebhookService
    {
        private readonly string _webhookSecret;

        public StripeWebhookService(IConfiguration config)
        {
            _webhookSecret = config["Stripe:WebhookSecret"]!;
        }

        // Método para validar la firma del webhook de Stripe y construir el evento a partir del payload recibido
        public Event ValidarWebhook(string payload, string signature)
        {
            return EventUtility.ConstructEvent(
                payload, signature, _webhookSecret);
        }
    }
}