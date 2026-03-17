using Microsoft.EntityFrameworkCore;
using SGC.Domain.Entities.Catalog;
using SGC.Domain.Interfaces.Repository;
using SGC.Persistence.Base;
using SGC.Persistence.Context;

namespace SGC.Persistence.Repositories.Catalog
{
    // Repositorio para ProveedorSalud, con metodos personalizados
    public class ProveedorSaludRepository : BaseRepository<ProveedorSalud>, IProveedorSaludRepository
    {
        public ProveedorSaludRepository(SGCDbContext context) : base(context) { }

        // Obtiene solo los proveedores de salud activos, ordenados por nombre
        public async Task<IEnumerable<ProveedorSalud>> GetActivosAsync()
        {
            return await Context.ProveedoresSalud
                .Where(p => p.Activo)
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }
    }
}