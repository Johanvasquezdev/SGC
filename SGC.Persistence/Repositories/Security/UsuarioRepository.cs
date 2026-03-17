using SGC.Domain.Entities.Security;
using SGC.Domain.Enums;
using SGC.Domain.Interfaces.Repository;
using SGC.Persistence.Base;
using SGC.Persistence.Context;

namespace SGC.Persistence.Repositories.Security
{
    // Repositorio de usuarios con metodos especificos para la entidad Usuario
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        private readonly SGCDbContext _context;

        public UsuarioRepository(SGCDbContext context) : base(context)
        {
            _context = context;
        }

        // Metodos especificos para la entidad Usuario
        public Task<Usuario> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        // Obtener usuarios por su rol (Administrador, Medico, Recepcionista)
        public Task<IEnumerable<Usuario>> GetByRolAsync(RolUsuario rol)
        {
            throw new NotImplementedException();
        }
    }
}
