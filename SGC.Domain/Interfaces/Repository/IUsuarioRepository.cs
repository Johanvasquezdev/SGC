using SGC.Domain.Entities.Security;

namespace SGC.Domain.Interfaces.Repository
{
    public interface IUsuarioRepository : IBaseRepository<Usuario> // Interfaz especifica para la entidad Usuario, que hereda de la interfaz generica IBaseRepository.
    {
        Task<Usuario> GetByEmailAsync(string email); // Obtener un usuario por su email, que es unico en el sistema.
        Task<IEnumerable<Usuario>> GetByRolAsync(string rol); // Obtener todos los usuarios con un rol especifico (Administrador, Medico, Paciente).
    }
}
