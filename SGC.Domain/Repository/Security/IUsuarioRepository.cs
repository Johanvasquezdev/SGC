using SGC.Domain.Entities.Security;

namespace SGC.Domain.Repository.Security
{
    public interface IUsuarioRepository : IBaseRepository<Usuario>
    {
        Task<Usuario> GetByEmailAsync(string email);
        Task<IEnumerable<Usuario>> GetByRolAsync(string rol);
    }
}
