using SGC.Domain.Entities.Appointments;
using SGC.Domain.Interfaces.Repository;
using SGC.Persistence.Base;
using SGC.Persistence.Context;

namespace SGC.Persistence.Repositories.Appointments
{
    public class CitaRepository : BaseRepository<Cita>, ICitaRepository
    {
        private readonly SGCDbContext _context;

        public CitaRepository(SGCDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<IEnumerable<Cita>> GetByPacienteIdAsync(int pacienteId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Cita>> GetByMedicoIdAsync(int medicoId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Cita>> GetByFechaAsync(DateTime fecha)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExisteConflictoAsync(int medicoId, DateTime fechaHora)
        {
            throw new NotImplementedException();
        }
    }
}
