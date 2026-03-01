using SGC.Domain.Entities.Catalog;
using SGC.Domain.Interfaces.Repository;
using SGC.Persistence.Base;
using SGC.Persistence.Context;

namespace SGC.Persistence.Repositories.Catalog
{
    public class EspecialidadRepository : BaseRepository<Especialidad>, IEspecialidadRepository
    {
        private readonly SGCDbContext _context;

        public EspecialidadRepository(SGCDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<IEnumerable<Especialidad>> GetActivasAsync()
        {
            throw new NotImplementedException();
        }
    }
}
