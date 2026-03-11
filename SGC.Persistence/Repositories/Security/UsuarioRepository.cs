using Microsoft.EntityFrameworkCore;
using SGC.Domain.Entities.Security;
using SGC.Domain.Enums;
using SGC.Domain.Interfaces.Repository;
using SGC.Persistence.Base;
using SGC.Persistence.Context;

namespace SGC.Persistence.Repositories.Security
{
    // Repositorio para operaciones de persistencia de usuarios
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(SGCDbContext context) : base(context) { }

        // Busca un usuario por su direccion de email
        public async Task<Usuario> GetByEmailAsync(string email)
        {
            return await Context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email)
                ?? throw new KeyNotFoundException($"No se encontro un usuario con email {email}.");
        }

        // Obtiene todos los usuarios que tienen un rol especifico
        public async Task<IEnumerable<Usuario>> GetByRolAsync(RolUsuario rol)
        {
            return await Context.Usuarios
                .Where(u => u.Rol == rol)
                .ToListAsync();
        }
    }
}
