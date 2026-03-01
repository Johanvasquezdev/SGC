using SGC.Domain.Entities.Medical;
using SGC.Domain.Interfaces.Repository;
using SGC.Persistence.Base;
using SGC.Persistence.Context;

namespace SGC.Persistence.Repositories.Medical
{
    public class MedicoRepository : BaseRepository<Medico>, IMedicoRepository
    {
        private readonly SGCDbContext _context;

        public MedicoRepository(SGCDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<Medico> GetByExequaturAsync(string exequatur)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Medico>> GetByEspecialidadAsync(int especialidadId)
        {
            throw new NotImplementedException();
        }
    }
}
