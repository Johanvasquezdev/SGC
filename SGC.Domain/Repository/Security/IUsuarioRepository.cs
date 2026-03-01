using SGC.Domain.Entities.Security;
using System.Threading.Tasks;

namespace SGC.Domain.Repository.Security
{
    // Interfaz de repositorio específica para la entidad Usuario.
    // Extiende las operaciones CRUD base con consultas propias del contexto de seguridad.
    public interface IUsuarioRepository : IBaseRepository<Usuario>
    {
        // Busca un usuario por su dirección de correo electrónico.
        // Usado principalmente en el proceso de autenticación.
        Task<Usuario> GetByEmailAsync(string email);
    }
}
