using SGC.Application.DTOs.Payments;
using SGC.Domain.Entities.Payments;

namespace SGC.Application.Mappers
{
    public static class PagoMapper
    {
        public static PagoResponse ToResponse(Pago pago)
        {
            return new PagoResponse
            {
                Id = pago.Id,
                CitaId = pago.CitaId,
                PacienteId = pago.PacienteId,
                Monto = pago.Monto,
                Moneda = pago.Moneda,
                Estado = pago.Estado.ToString(),
                StripePaymentIntentId = pago.StripePaymentIntentId,
                FechaPago = pago.FechaPago,
                FechaCreacion = pago.FechaCreacion
            };
        }
    }
}