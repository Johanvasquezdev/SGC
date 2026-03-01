using SGC.Domain.Entities.Notifications;
using SGC.Domain.Interfaces.Repository;
using SGC.Persistence.Base;
using SGC.Persistence.Context;

namespace SGC.Persistence.Repositories.Notifications
{
    public class PrefNotificacionRepository : BaseRepository<PrefNotificacion>, IPrefNotificacionRepository
    {
        private readonly SGCDbContext _context;

        public PrefNotificacionRepository(SGCDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<PrefNotificacion> GetByUsuarioIdAsync(int usuarioId)
        {
            throw new NotImplementedException();
        }
    }
}
