using SGC.Application.Contracts;
using SGC.Application.DTOs.Notifications;
using SGC.Domain.Entities.Notifications;
using SGC.Domain.Interfaces.Repository;
using SGC.Domain.Validators;

namespace SGC.Application.Services
{
    // Servicio de aplicacion para la gestion de preferencias de notificacion
    public class PrefNotificacionService : IPrefNotificacionService
    {
        // Repositorio de preferencias de notificacion para acceso a datos
        private readonly IPrefNotificacionRepository _prefRepository;

        // Validador de reglas de negocio para preferencias de notificacion
        private readonly PrefNotificacionValidator _validator = new PrefNotificacionValidator();

        public PrefNotificacionService(IPrefNotificacionRepository prefRepository)
        {
            _prefRepository = prefRepository;
        }

        // Obtiene las preferencias de notificacion de un usuario
        public async Task<PrefNotificacionResponse> GetByUsuarioAsync(int usuarioId)
        {
            var pref = await _prefRepository.GetByUsuarioIdAsync(usuarioId);
            return MapToResponse(pref);
        }

        // Crea o actualiza las preferencias de notificacion de un usuario
        public async Task<PrefNotificacionResponse> GuardarAsync(PrefNotificacionRequest request)
        {
            PrefNotificacion pref;

            try
            {
                // Intentar obtener las preferencias existentes del usuario
                pref = await _prefRepository.GetByUsuarioIdAsync(request.UsuarioId);

                // Actualizar las preferencias existentes
                pref.RecibirEmail = request.RecibirEmail;
                pref.RecibirSMS = request.RecibirSMS;
                pref.RecibirPush = request.RecibirPush;
                pref.HorasAntesRecordatorio = request.HorasAntesRecordatorio;

                // Validar reglas de negocio antes de actualizar
                _validator.Validar(pref);

                await _prefRepository.UpdateAsync(pref);
            }
            catch (KeyNotFoundException)
            {
                // Si no existen, crear nuevas preferencias para el usuario
                pref = new PrefNotificacion
                {
                    UsuarioId = request.UsuarioId,
                    RecibirEmail = request.RecibirEmail,
                    RecibirSMS = request.RecibirSMS,
                    RecibirPush = request.RecibirPush,
                    HorasAntesRecordatorio = request.HorasAntesRecordatorio
                };

                // Validar reglas de negocio antes de guardar
                _validator.Validar(pref);

                await _prefRepository.AddAsync(pref);
            }

            return MapToResponse(pref);
        }

        // Convierte una entidad PrefNotificacion a su DTO de respuesta
        private static PrefNotificacionResponse MapToResponse(PrefNotificacion p)
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
