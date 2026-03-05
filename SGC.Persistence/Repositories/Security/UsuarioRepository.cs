using Microsoft.EntityFrameworkCore;
using SGC.Domain.Entities.Security;
using SGC.Domain.Repository.Security;
using SGC.Persistence.Context;

namespace SGC.Persistence.Repositories.Security
{
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(SGCDbContext context) : base(context) { }

        public async Task<Usuario> GetByEmailAsync(string email)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email)
                ?? throw new KeyNotFoundException($"No se encontró un usuario con el email '{email}'.");
        }

        public async Task<IEnumerable<Usuario>> GetByRolAsync(string rol)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .Where(u => u.Rol.ToString() == rol)
                .ToListAsync();
        }
    }
}
