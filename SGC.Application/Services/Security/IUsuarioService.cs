using SGC.Application.DTOs.Security;

namespace SGC.Application.Services.Security
{
    public interface IUsuarioService
    {
        Task<UsuarioDto> GetByIdAsync(int id);
        Task<IEnumerable<UsuarioDto>> GetAllAsync();
        Task<UsuarioDto> GetByEmailAsync(string email);
        Task<IEnumerable<UsuarioDto>> GetByRolAsync(string rol);
        Task<UsuarioDto> CreateAsync(CreateUsuarioRequest request);
        Task<UsuarioDto> UpdateAsync(int id, UpdateUsuarioRequest request);
        Task DeleteAsync(int id);
    }
}
