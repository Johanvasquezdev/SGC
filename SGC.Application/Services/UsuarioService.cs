using SGC.Application.Contracts;
using SGC.Application.DTOs.Security;
using SGC.Domain.Entities.Security;
using SGC.Domain.Enums;
using SGC.Domain.Interfaces.Repository;

namespace SGC.Application.Services
{
    // Servicio de aplicacion para la gestion de usuarios del sistema
    public class UsuarioService : IUsuarioService
    {
        // Repositorio de usuarios para acceso a datos
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        // Obtiene un usuario por su identificador
        public async Task<UsuarioResponse> GetByIdAsync(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            return MapToResponse(usuario);
        }

        // Obtiene todos los usuarios del sistema
        public async Task<IEnumerable<UsuarioResponse>> GetAllAsync()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            return usuarios.Select(MapToResponse);
        }

        // Busca un usuario por su direccion de correo electronico
        public async Task<UsuarioResponse> GetByEmailAsync(string email)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(email);
            return MapToResponse(usuario);
        }

        // Obtiene todos los usuarios que tienen un rol especifico (Administrador, Medico, Paciente)
        public async Task<IEnumerable<UsuarioResponse>> GetByRolAsync(string rol)
        {
            // Convertir el string del rol al enum correspondiente
            if (!Enum.TryParse<RolUsuario>(rol, true, out var rolEnum))
                throw new ArgumentException($"El rol '{rol}' no es valido.");

            var usuarios = await _usuarioRepository.GetByRolAsync(rolEnum);
            return usuarios.Select(MapToResponse);
        }

        // Desactiva un usuario del sistema usando la regla de dominio
        public async Task DesactivarAsync(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            usuario.Desactivar();
            await _usuarioRepository.UpdateAsync(usuario);
        }

        // Activa un usuario en el sistema usando la regla de dominio
        public async Task ActivarAsync(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            usuario.Activar();
            await _usuarioRepository.UpdateAsync(usuario);
        }

        // Convierte una entidad Usuario a su DTO de respuesta
        private static UsuarioResponse MapToResponse(Usuario usuario)
        {
            return new UsuarioResponse
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Rol = usuario.Rol.ToString(),
                FechaCreacion = usuario.FechaCreacion,
                Activo = usuario.Activo
            };
        }
    }
}
