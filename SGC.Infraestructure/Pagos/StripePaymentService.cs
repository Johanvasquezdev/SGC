using Microsoft.Extensions.Configuration;
using SGC.Domain.Interfaces;
using Stripe;

namespace SGC.Infraestructure.Pagos
{
    // Servicio de pago utilizando Stripe para procesar pagos de citas médicas
    public class StripePaymentService : IPaymentService
    {
        public StripePaymentService(IConfiguration config)
        {
            StripeConfiguration.ApiKey = config["Stripe:SecretKey"];
        }

        // Método para crear un intento de pago en Stripe y obtener el client secret para completar el pago en el frontend
        public async Task<string> CrearIntentoPagoAsync(
            decimal monto, string moneda, int citaId)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(monto * 100),
                Currency = moneda.ToLower(),
                Metadata = new Dictionary<string, string>
                {
                    { "citaId", citaId.ToString() }
                }
            };
            var service = new PaymentIntentService();
            var intent = await service.CreateAsync(options);
            return intent.ClientSecret;
        }

        // Método para confirmar el estado del pago después de que el cliente complete el proceso de pago en el frontend
        public async Task<bool> ConfirmarPagoAsync(
            string paymentIntentId)
        {
            var service = new PaymentIntentService();
            var intent = await service.GetAsync(paymentIntentId);
            return intent.Status == "succeeded";
        }

        // metodo para reembolsar un pago en caso de cancelación de cita o devolución de dinero al paciente
        public async Task<bool> ReembolsarPagoAsync(
            string paymentIntentId)
        {
            var options = new RefundCreateOptions
            {
                PaymentIntent = paymentIntentId
            };
            var service = new RefundService();
            var refund = await service.CreateAsync(options);
            return refund.Status == "succeeded";
        }
    }
}
