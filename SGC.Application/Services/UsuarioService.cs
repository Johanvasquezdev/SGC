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
            return await ExecuteOperacionAsync(
                "GetUsuarioById",
                async () =>
                {
                    var usuario = await _usuarioRepository.GetByIdAsync(id);
                    return MapToResponse(usuario);
                },
                $"UsuarioId: {id}");
        }

        // Obtiene todos los usuarios del sistema
        public async Task<IEnumerable<UsuarioResponse>> GetAllAsync()
        {
            return await ExecuteOperacionAsync(
                "GetAllUsuarios",
                async () =>
                {
                    var usuarios = await _usuarioRepository.GetAllAsync();
                    return usuarios.Select(MapToResponse);
                });
        }

        // Obtiene un usuario por su email
        public async Task<UsuarioResponse> GetByEmailAsync(string email)
        {
            return await ExecuteOperacionAsync(
                "GetUsuarioByEmail",
                async () =>
                {
                    var usuario = await _usuarioRepository.GetByEmailAsync(email);
                    return MapToResponse(usuario);
                },
                $"Email: {email}");
        }

        // Obtiene usuarios filtrados por rol
        public async Task<IEnumerable<UsuarioResponse>> GetByRolAsync(string rol)
        {
            return await ExecuteOperacionAsync(
                "GetUsuariosByRol",
                async () =>
                {
                    if (!Enum.TryParse<RolUsuario>(rol, true, out var rolEnum))
                    {
                        throw new ArgumentException($"El rol '{rol}' no es válido.");
                    }

                    var usuarios = await _usuarioRepository.GetByRolAsync(rolEnum);
                    return usuarios.Select(MapToResponse);
                },
                $"Rol: {rol}");
        }

        // Desactiva un usuario usando la regla de dominio
        public async Task DesactivarAsync(int id)
        {
            await ExecuteOperacionAsync(
                "DesactivarUsuario",
                async () =>
                {
                    var usuario = await _usuarioRepository.GetByIdAsync(id);
                    usuario.Desactivar();
                    await _usuarioRepository.UpdateAsync(usuario);
                },
                $"Id: {id}");
        }

        // Activa un usuario usando la regla de dominio
        public async Task ActivarAsync(int id)
        {
            await ExecuteOperacionAsync(
                "ActivarUsuario",
                async () =>
                {
                    var usuario = await _usuarioRepository.GetByIdAsync(id);
                    usuario.Activar();
                    await _usuarioRepository.UpdateAsync(usuario);
                },
                $"Id: {id}");
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
