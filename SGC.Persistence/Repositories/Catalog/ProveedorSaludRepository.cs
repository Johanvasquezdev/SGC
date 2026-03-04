using Microsoft.EntityFrameworkCore;
using SGC.Domain.Entities.Catalog;
using SGC.Domain.Interfaces.Repository;
using SGC.Persistence.Base;
using SGC.Persistence.Context;

namespace SGC.Persistence.Repositories.Catalog
{
    public class ProveedorSaludRepository : BaseRepository<ProveedorSalud>, IProveedorSaludRepository
    {
        private readonly SGCDbContext _context;

        public ProveedorSaludRepository(SGCDbContext context) : base(context)
        {
            _context = context;
        }

        // Retorna solo los proveedores de salud con Activo == true
        public async Task<IEnumerable<ProveedorSalud>> GetActivosAsync()
        {
            return await _context.ProveedoresSalud
                .Where(p => p.Activo)
                .ToListAsync();
        }
    }
}
