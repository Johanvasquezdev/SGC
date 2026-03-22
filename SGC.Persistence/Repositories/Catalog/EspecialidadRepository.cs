using Microsoft.EntityFrameworkCore;
using SGC.Domain.Entities.Catalog;
using SGC.Domain.Interfaces.Repository;
using SGC.Persistence.Base;
using SGC.Persistence.Context;

namespace SGC.Persistence.Repositories.Catalog
{
    // Repositorio para operaciones de persistencia de especialidades medicas
    public class EspecialidadRepository : BaseRepository<Especialidad>, IEspecialidadRepository
    {
        public EspecialidadRepository(SGCDbContext context) : base(context) { }

        // Obtiene solo las especialidades que estan activas en el sistema
        public async Task<IEnumerable<Especialidad>> GetActivasAsync()
        {
            return await Context.Especialidades
                .Where(e => e.Activo)
                .ToListAsync();
        }
    }
}
