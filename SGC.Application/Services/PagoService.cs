using SGC.Application.Contracts;
using SGC.Application.DTOs.Payments;
using SGC.Application.Mappers;
using SGC.Domain.Entities.Payments;
using SGC.Domain.Interfaces;
using SGC.Domain.Interfaces.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGC.Application.Services
{
    // Servicio de aplicación para gestionar los pagos relacionados con las citas médicas. Implementa la interfaz IPagoService y utiliza el repositorio de pagos y el servicio de pagos para realizar las operaciones necesarias.
    public class PagoService : IPagoService
    {
        private readonly IPagoRepository _pagoRepo;
        private readonly IPaymentService _paymentService;
        private readonly ISGCLogger _logger;

        public PagoService(IPagoRepository pagoRepo,
            IPaymentService paymentService,
            ISGCLogger logger)
        {
            _pagoRepo = pagoRepo;
            _paymentService = paymentService;
            _logger = logger;
        }

        // Crea un intento de pago para una cita médica, registrando el pago en la base de datos y devolviendo el client secret generado por el servicio de pagos para que el cliente
        public async Task<string> CrearIntentoPagoAsync(
            CrearPagoRequest request)
        {
            _logger.LogInfo(
                $"Creando intento de pago para cita {request.CitaId}");

            var clientSecret = await _paymentService
                .CrearIntentoPagoAsync(
                    request.Monto,
                    request.Moneda,
                    request.CitaId);

            var pago = new Pago
            {
                CitaId = request.CitaId,
                PacienteId = request.PacienteId,
                Monto = request.Monto,
                Moneda = request.Moneda,
                StripePaymentIntentId = clientSecret
            };

            await _pagoRepo.AddAsync(pago);
            return clientSecret;
        }

        // Confirma un pago utilizando el ID del intento de pago de Stripe. Si la confirmación es exitosa, actualiza el estado del pago en la base de datos a "Confirmado".
        public async Task<bool> ConfirmarPagoAsync(
            string stripePaymentIntentId)
        {
            var confirmado = await _paymentService
                .ConfirmarPagoAsync(stripePaymentIntentId);

            if (confirmado)
            {
                var pago = await _pagoRepo
                    .GetByStripeIdAsync(stripePaymentIntentId);
                if (pago != null)
                {
                    pago.Confirmar();
                    await _pagoRepo.UpdateAsync(pago);
                }
            }

            return confirmado;
        }

        // Reembolsa un pago utilizando el ID del pago. Si el reembolso es exitoso, actualiza el estado del pago en la base de datos a "Reembolsado".
        public async Task<bool> ReembolsarPagoAsync(int pagoId)
        {
            var pago = await _pagoRepo.GetByIdAsync(pagoId);
            var reembolsado = await _paymentService
                .ReembolsarPagoAsync(pago.StripePaymentIntentId!);

            if (reembolsado)
            {
                pago.Reembolsar();
                await _pagoRepo.UpdateAsync(pago);
            }

            return reembolsado;
        }

        // Obtiene el pago asociado a una cita médica específica utilizando el ID de la cita. Devuelve un objeto PagoResponse que contiene los detalles del pago, o null si no se encuentra ningún pago para esa cita.
        public async Task<PagoResponse?> GetByCitaAsync(int citaId)
        {
            var pago = await _pagoRepo.GetByCitaAsync(citaId);
            return pago == null ? null : PagoMapper.ToResponse(pago);
        }

        // Obtiene todos los pagos realizados por un paciente específico utilizando el ID del paciente. Devuelve una lista de objetos PagoResponse que contienen los detalles de cada pago realizado por el paciente.
        public async Task<IEnumerable<PagoResponse>> GetByPacienteAsync(
            int pacienteId)
        {
            var pagos = await _pagoRepo.GetByPacienteAsync(pacienteId);
            return pagos.Select(PagoMapper.ToResponse);
        }
    }
}