using SGC.Domain.Interfaces;
using System.Threading.Tasks;

namespace SGC.Infraestructure.Pagos
{
    // Implementación mínima de pagos para desarrollo y pruebas.
    public class StripePaymentService : IPaymentService
    {
        // Simula la creación de un intento de pago y devuelve un ID ficticio.
        public Task<string> CrearIntentoPagoAsync(decimal monto, string moneda, int citaId)
        {
            return Task.FromResult($"pi_test_{citaId}_{monto}_{moneda}");
        }

        // Simula la confirmación de un pago.
        public Task<bool> ConfirmarPagoAsync(string paymentIntentId)
        {
            return Task.FromResult(true);
        }

        // Simula el reembolso de un pago.
        public Task<bool> ReembolsarPagoAsync(string paymentIntentId)
        {
            return Task.FromResult(true);
        }
    }
}
