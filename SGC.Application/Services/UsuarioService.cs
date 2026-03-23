using SGC.Application.Contracts;
using SGC.Application.DTOs.Security;
using SGC.Application.Services.Base;
using SGC.Domain.Enums;
using SGC.Domain.Interfaces.ILogger;
using SGC.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGC.Application.Services
{
    // Servicio para gestionar usuarios del sistema
    public class UsuarioService : BaseService, IUsuarioService
    {
        // Repositorio para acceso a datos de usuarios
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(
            IUsuarioRepository usuarioRepository,
            ISGCLogger logger) : base(logger)
        {
            _usuarioRepository = usuarioRepository;
        }

        // Obtiene un usuario por su ID
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

        // Obtiene un usuario por su email
        public async Task<UsuarioResponse> GetByEmailAsync(string email)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(email);
            return MapToResponse(usuario);
        }

        // Obtiene usuarios filtrados por rol
        public async Task<IEnumerable<UsuarioResponse>> GetByRolAsync(string rol)
        {
            if (!Enum.TryParse<RolUsuario>(rol, true, out var rolEnum))
                throw new ArgumentException($"El rol '{rol}' no es válido.");

            var usuarios = await _usuarioRepository.GetByRolAsync(rolEnum);
            return usuarios.Select(MapToResponse);
        }

        // Desactiva un usuario usando la regla de dominio
        public async Task DesactivarAsync(int id)
        {
            LogAdvertencia("DesactivarUsuario", $"Id: {id}");
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            usuario.Desactivar();
            await _usuarioRepository.UpdateAsync(usuario);
        }

        // Activa un usuario usando la regla de dominio
        public async Task ActivarAsync(int id)
        {
            LogOperacion("ActivarUsuario", $"Id: {id}");
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            usuario.Activar();
            await _usuarioRepository.UpdateAsync(usuario);
        }

        // Convierte una entidad Usuario a su DTO de respuesta
        private static UsuarioResponse MapToResponse(
            Domain.Entities.Security.Usuario usuario)
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