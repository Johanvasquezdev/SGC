using Microsoft.EntityFrameworkCore;
using SGC.Domain.Entities.Notifications;
using SGC.Domain.Interfaces.Repository;
using SGC.Persistence.Base;
using SGC.Persistence.Context;

namespace SGC.Persistence.Repositories.Notifications
{
    public class PrefNotificacionRepository : BaseRepository<PrefNotificacion>, IPrefNotificacionRepository
    {
        public PrefNotificacionRepository(SGCDbContext context) : base(context) { }

        // Obtiene las preferencias de notificacion de un usuario por su ID. Lanza una excepcion si no se encuentran preferencias para ese usuario.
        public async Task<PrefNotificacion> GetByUsuarioIdAsync(int usuarioId)
        {
            return await Context.PrefNotificaciones
                       .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId)
                   ?? throw new KeyNotFoundException(
                       $"No se encontraron preferencias para usuario {usuarioId}.");
        }
    }
}