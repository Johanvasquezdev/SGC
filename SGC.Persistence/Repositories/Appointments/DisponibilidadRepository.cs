using SGC.Domain.Entities.Appointments;
using SGC.Domain.Interfaces.Repository;
using SGC.Persistence.Base;
using SGC.Persistence.Context;

namespace SGC.Persistence.Repositories.Appointments
{
    public class DisponibilidadRepository : BaseRepository<Disponibilidad>, IDisponibilidadRepository
    {
        private readonly SGCDbContext _context;

        public DisponibilidadRepository(SGCDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<IEnumerable<Disponibilidad>> GetByMedicoIdAsync(int medicoId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Disponibilidad>> GetByDiaAsync(string diaSemana)
        {
            throw new NotImplementedException();
        }
    }
}
