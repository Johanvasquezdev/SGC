using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGC.Domain.Entities.Security;
using SGC.Domain.Repository;
using SGC.Persistence.Context;
using SGC.Persistence.Repositories.Common;

namespace SGC.Persistence.Repositories.Security
{
    // Implementación EF Core del repositorio de usuarios.
    // Cubre el módulo de seguridad (feature/persistencia-security).
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(AppDbContext context) : base(context) { }

        // Busca un usuario por email para el flujo de autenticación JWT.
        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
