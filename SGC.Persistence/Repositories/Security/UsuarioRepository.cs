using Microsoft.EntityFrameworkCore;
using SGC.Domain.Entities.Security;
using SGC.Domain.Enums;
using SGC.Domain.Interfaces.Repository;
using SGC.Persistence.Base;
using SGC.Persistence.Context;

namespace SGC.Persistence.Repositories.Security
{
    // Repositorio para la entidad Usuario con consultas personalizadas
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(SGCDbContext context) : base(context) { }

        public async Task<Usuario> GetByEmailAsync(string email)
        {
            return await Context.Usuarios
                       .FirstOrDefaultAsync(u => u.Email == email)
                   ?? throw new KeyNotFoundException(
                       $"No se encontró usuario con email {email}.");
        }

        public async Task<IEnumerable<Usuario>> GetByRolAsync(RolUsuario rol)
        {
            return await Context.Usuarios
                .Where(u => u.Rol == rol)
                .OrderBy(u => u.Nombre)
                .ToListAsync();
        }
    }
}