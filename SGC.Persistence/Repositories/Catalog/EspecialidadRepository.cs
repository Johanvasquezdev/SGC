using Microsoft.EntityFrameworkCore;
using SGC.Domain.Entities.Catalog;
using SGC.Domain.Interfaces.Repository;
using SGC.Persistence.Base;
using SGC.Persistence.Context;

namespace SGC.Persistence.Repositories.Catalog
{
    // Repositorio para la entidad Especialidad, con metodos personalizados
    public class EspecialidadRepository : BaseRepository<Especialidad>, IEspecialidadRepository
    {
        public EspecialidadRepository(SGCDbContext context) : base(context) { }

        // Obtiene todas las especialidades activas, ordenadas por nombre
        public async Task<IEnumerable<Especialidad>> GetActivasAsync()
        {
            return await Context.Especialidades
                .Where(e => e.Activo)
                .OrderBy(e => e.Nombre)
                .ToListAsync();
        }
    }
}