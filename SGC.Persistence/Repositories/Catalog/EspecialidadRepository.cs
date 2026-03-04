using Microsoft.EntityFrameworkCore;
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

        // Retorna solo las especialidades con Activo == true
        public async Task<IEnumerable<Especialidad>> GetActivasAsync()
        {
            return await _context.Especialidades
                .Where(e => e.Activo)
                .ToListAsync();
        }
    }
}
