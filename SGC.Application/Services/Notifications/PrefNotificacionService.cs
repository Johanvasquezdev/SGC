using SGC.Application.DTOs.Notifications;
using SGC.Domain.Entities.Notifications;
using SGC.Domain.Repository.Notifications;

namespace SGC.Application.Services.Notifications
{
    public class PrefNotificacionService : IPrefNotificacionService
    {
        private readonly IPrefNotificacionRepository _repository;

        public PrefNotificacionService(IPrefNotificacionRepository repository)
        {
            _repository = repository;
        }

        public async Task<PrefNotificacionDto> GetByUsuarioIdAsync(int usuarioId)
        {
            var pref = await _repository.GetByUsuarioIdAsync(usuarioId);
            return MapToDto(pref);
        }

        public async Task<PrefNotificacionDto> CreateAsync(int usuarioId)
        {
            var pref = new PrefNotificacion
            {
                UsuarioId = usuarioId,
                RecibirEmail = true,
                RecibirSMS = false,
                RecibirPush = true
            };
            await _repository.AddAsync(pref);
            return MapToDto(pref);
        }

        public async Task<PrefNotificacionDto> UpdateAsync(int usuarioId, UpdatePrefNotificacionRequest request)
        {
            var pref = await _repository.GetByUsuarioIdAsync(usuarioId);
            pref.RecibirEmail = request.RecibirEmail;
            pref.RecibirSMS = request.RecibirSMS;
            pref.RecibirPush = request.RecibirPush;
            await _repository.UpdateAsync(pref);
            return MapToDto(pref);
        }

        private static PrefNotificacionDto MapToDto(PrefNotificacion p) => new PrefNotificacionDto
        {
            Id = p.Id,
            UsuarioId = p.UsuarioId,
            RecibirEmail = p.RecibirEmail,
            RecibirSMS = p.RecibirSMS,
            RecibirPush = p.RecibirPush
        };
    }
}
