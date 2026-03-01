using SGC.Domain.Base;
using SGC.Domain.Interfaces.Repository;
using SGC.Persistence.Base;
using SGC.Persistence.Context;

namespace SGC.Persistence.Repositories.Audit
{
    public class AuditoriaRepository : BaseRepository<AuditEntity>, IAuditoriaRepository
    {
        private readonly SGCDbContext _context;

        public AuditoriaRepository(SGCDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<IEnumerable<AuditEntity>> GetByEntidadAsync(string entidad)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AuditEntity>> GetByUsuarioIdAsync(int usuarioId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AuditEntity>> GetByFechaAsync(DateTime fecha)
        {
            throw new NotImplementedException();
        }
    }
}
