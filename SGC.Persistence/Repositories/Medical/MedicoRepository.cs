using Microsoft.EntityFrameworkCore;
using SGC.Domain.Entities.Medical;
using SGC.Domain.Repository.Medical;
using SGC.Persistence.Context;

namespace SGC.Persistence.Repositories.Medical
{
    public class MedicoRepository : BaseRepository<Medico>, IMedicoRepository
    {
        public MedicoRepository(SGCDbContext context) : base(context) { }

        public async Task<Medico> GetByExequaturAsync(string exequatur)
        {
            return await _context.Medicos
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Exequatur == exequatur)
                ?? throw new KeyNotFoundException($"No se encontró un médico con el exequatur '{exequatur}'.");
        }

        public async Task<IEnumerable<Medico>> GetByEspecialidadAsync(int especialidadId)
        {
            return await _context.Medicos
                .AsNoTracking()
                .Where(m => m.EspecialidadId == especialidadId)
                .ToListAsync();
        }
    }
}
