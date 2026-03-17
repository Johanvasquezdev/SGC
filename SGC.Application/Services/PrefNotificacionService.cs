using SGC.Application.Contracts;
using SGC.Application.DTOs.Notifications;
using SGC.Application.Services.Base;
using SGC.Domain.Entities.Notifications;
using SGC.Domain.Interfaces.ILogger;
using SGC.Domain.Interfaces.Repository;
using SGC.Domain.Validators;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGC.Application.Services
{
    public class PrefNotificacionService : BaseService, IPrefNotificacionService
    {
        private readonly IPrefNotificacionRepository _prefRepository;
        private readonly PrefNotificacionValidator _validator;

        public PrefNotificacionService(
            IPrefNotificacionRepository prefRepository,
            PrefNotificacionValidator validator,
            ISGCLogger logger) : base(logger)
        {
            _prefRepository = prefRepository;
            _validator = validator;
        }

        public async Task<PrefNotificacionResponse> GetByUsuarioAsync(
            int usuarioId)
        {
            var pref = await _prefRepository.GetByUsuarioIdAsync(usuarioId);
            return MapToResponse(pref);
        }

        public async Task<PrefNotificacionResponse> GuardarAsync(
            PrefNotificacionRequest request)
        {
            LogOperacion("GuardarPreferencias",
                $"UsuarioId: {request.UsuarioId}");
            PrefNotificacion pref;
            try
            {
                pref = await _prefRepository
                    .GetByUsuarioIdAsync(request.UsuarioId);
                pref.RecibirEmail = request.RecibirEmail;
                pref.RecibirSMS = request.RecibirSMS;
                pref.RecibirPush = request.RecibirPush;
                pref.HorasAntesRecordatorio = request.HorasAntesRecordatorio;
                _validator.Validar(pref);
                await _prefRepository.UpdateAsync(pref);
            }
            catch (KeyNotFoundException)
            {
                pref = new PrefNotificacion
                {
                    UsuarioId = request.UsuarioId,
                    RecibirEmail = request.RecibirEmail,
                    RecibirSMS = request.RecibirSMS,
                    RecibirPush = request.RecibirPush,
                    HorasAntesRecordatorio = request.HorasAntesRecordatorio
                };
                _validator.Validar(pref);
                await _prefRepository.AddAsync(pref);
            }
            return MapToResponse(pref);
        }

        private static PrefNotificacionResponse MapToResponse(
            PrefNotificacion p)
        {
            return new PrefNotificacionResponse
            {
                Id = p.Id,
                UsuarioId = p.UsuarioId,
                RecibirEmail = p.RecibirEmail,
                RecibirSMS = p.RecibirSMS,
                RecibirPush = p.RecibirPush,
                HorasAntesRecordatorio = p.HorasAntesRecordatorio
            };
        }
    }
}