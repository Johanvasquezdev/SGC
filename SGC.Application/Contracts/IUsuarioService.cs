using SGC.Application.DTOs.Security;

namespace SGC.Application.Contracts
{
    // Contrato para las operaciones de gestion de usuarios
    public interface IUsuarioService
    {
        // Obtiene un usuario por su identificador
        Task<UsuarioResponse> GetByIdAsync(int id);

        // Obtiene todos los usuarios
        Task<IEnumerable<UsuarioResponse>> GetAllAsync();

        // Busca un usuario por su correo electronico
        Task<UsuarioResponse> GetByEmailAsync(string email);

        // Obtiene todos los usuarios con un rol especifico
        Task<IEnumerable<UsuarioResponse>> GetByRolAsync(string rol);

        // Desactiva un usuario del sistema
        Task DesactivarAsync(int id);

        // Activa un usuario en el sistema
        Task ActivarAsync(int id);
    }
}
