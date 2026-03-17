using SGC.Application.Contracts;
using SGC.Application.DTOs.Payments;
using SGC.Application.Mappers;
using SGC.Application.Services.Base;
using SGC.Domain.Entities.Payments;
using SGC.Domain.Interfaces;
using SGC.Domain.Interfaces.ILogger;
using SGC.Domain.Interfaces.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGC.Application.Services
{
    public class PagoService : BaseService, IPagoService
    {
        private readonly IPagoRepository _pagoRepo;
        private readonly IPaymentService _paymentService;

        public PagoService(
            IPagoRepository pagoRepo,
            IPaymentService paymentService,
            ISGCLogger logger) : base(logger)
        {
            _pagoRepo = pagoRepo;
            _paymentService = paymentService;
        }

        public async Task<string> CrearIntentoPagoAsync(
            CrearPagoRequest request)
        {
            LogOperacion("CrearIntentoPago",
                $"CitaId: {request.CitaId}, Monto: {request.Monto}");

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

        public async Task<bool> ConfirmarPagoAsync(
            string stripePaymentIntentId)
        {
            LogOperacion("ConfirmarPago",
                $"StripeId: {stripePaymentIntentId}");

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

        public async Task<bool> ReembolsarPagoAsync(int pagoId)
        {
            LogAdvertencia("ReembolsarPago", $"PagoId: {pagoId}");

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

        public async Task<PagoResponse?> GetByCitaAsync(int citaId)
        {
            var pago = await _pagoRepo.GetByCitaAsync(citaId);
            return pago == null ? null : PagoMapper.ToResponse(pago);
        }

        public async Task<IEnumerable<PagoResponse>> GetByPacienteAsync(
            int pacienteId)
        {
            var pagos = await _pagoRepo.GetByPacienteAsync(pacienteId);
            return pagos.Select(PagoMapper.ToResponse);
        }
    }
}