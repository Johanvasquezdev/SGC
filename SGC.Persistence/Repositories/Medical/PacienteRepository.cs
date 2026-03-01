using SGC.Domain.Entities.Medical;
using SGC.Domain.Interfaces.Repository;
using SGC.Persistence.Base;
using SGC.Persistence.Context;

namespace SGC.Persistence.Repositories.Medical
{
    public class PacienteRepository : BaseRepository<Paciente>, IPacienteRepository
    {
        private readonly SGCDbContext _context;

        public PacienteRepository(SGCDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<Paciente> GetByCedulaAsync(string cedula)
        {
            throw new NotImplementedException();
        }
    }
}
