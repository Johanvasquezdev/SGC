using System.Security.Cryptography;
using System.Text;
using SGC.Application.DTOs.Security;
using SGC.Domain.Entities.Security;
using SGC.Domain.Repository.Security;

namespace SGC.Application.Services.Security
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        public async Task<UsuarioDto> GetByIdAsync(int id)
        {
            var usuario = await _repository.GetByIdAsync(id);
            return MapToDto(usuario);
        }

        public async Task<IEnumerable<UsuarioDto>> GetAllAsync()
        {
            var usuarios = await _repository.GetAllAsync();
            return usuarios.Select(MapToDto);
        }

        public async Task<UsuarioDto> GetByEmailAsync(string email)
        {
            var usuario = await _repository.GetByEmailAsync(email);
            return MapToDto(usuario);
        }

        public async Task<IEnumerable<UsuarioDto>> GetByRolAsync(string rol)
        {
            var usuarios = await _repository.GetByRolAsync(rol);
            return usuarios.Select(MapToDto);
        }

        public async Task<UsuarioDto> CreateAsync(CreateUsuarioRequest request)
        {
            var usuario = new Usuario
            {
                Nombre = request.Nombre,
                Email = request.Email,
                PasswordHash = HashPassword(request.Password),
                Rol = Enum.Parse<RolUsuario>(request.Rol, ignoreCase: true),
                FechaCreacion = DateTime.UtcNow
            };
            await _repository.AddAsync(usuario);
            return MapToDto(usuario);
        }

        public async Task<UsuarioDto> UpdateAsync(int id, UpdateUsuarioRequest request)
        {
            var usuario = await _repository.GetByIdAsync(id);
            usuario.Nombre = request.Nombre;
            usuario.Email = request.Email;
            usuario.Rol = Enum.Parse<RolUsuario>(request.Rol, ignoreCase: true);
            await _repository.UpdateAsync(usuario);
            return MapToDto(usuario);
        }

        public async Task DeleteAsync(int id)
        {
            var usuario = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(usuario);
        }

        private static UsuarioDto MapToDto(Usuario u) => new UsuarioDto
        {
            Id = u.Id,
            Nombre = u.Nombre,
            Email = u.Email,
            Rol = u.Rol.ToString(),
            FechaCreacion = u.FechaCreacion
        };

        private static string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(16);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password), salt,
                iterations: 100_000, HashAlgorithmName.SHA256, outputLength: 32);
            return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }
    }
}
