using System.Threading.Tasks;
using SGC.Domain.Entities.Security;

namespace SGC.Domain.Repository
{
    // Repositorio específico para la entidad Usuario.
    // Extiende las operaciones CRUD base con consultas propias del módulo de seguridad.
    public interface IUsuarioRepository : IBaseRepository<Usuario>
    {
        // Busca un usuario por su dirección de correo electrónico.
        // Utilizado en el flujo de autenticación JWT para validar credenciales.
        Task<Usuario?> GetByEmailAsync(string email);
    }
}
