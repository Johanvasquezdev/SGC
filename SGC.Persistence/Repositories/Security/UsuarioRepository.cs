using SGC.Domain.Entities.Security;
using SGC.Domain.Interfaces.Repository;
using SGC.Persistence.Base;
using SGC.Persistence.Context;

namespace SGC.Persistence.Repositories.Security
{
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        private readonly SGCDbContext _context;

        public UsuarioRepository(SGCDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<Usuario> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Usuario>> GetByRolAsync(string rol)
        {
            throw new NotImplementedException();
        }
    }
}
