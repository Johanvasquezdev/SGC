using BCrypt.Net;
using SGC.Application.Contracts;
using SGC.Application.DTOs.Security;
using SGC.Application.Services.Base;
using SGC.Domain.Interfaces.ILogger;
using SGC.Domain.Interfaces.Repository;
using System;
using System.Threading.Tasks;

namespace SGC.Application.Services
{
    // Servicio de autenticacion para el sistema
    public class AuthService : BaseService, IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;

        public AuthService(
            IUsuarioRepository usuarioRepository,
            ITokenService tokenService,
            ISGCLogger logger) : base(logger)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
        }

        // Metodo para autenticar a un usuario y generar un token JWT
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            LogOperacion("Login", $"Email: {request.Email}");

            var usuario = await _usuarioRepository
                .GetByEmailAsync(request.Email);

            // Validaciones de seguridad
            if (usuario == null)
                throw new UnauthorizedAccessException(
                    "Credenciales incorrectas.");

            if (!usuario.Activo)
                throw new UnauthorizedAccessException(
                    "El usuario está desactivado.");

            // Verifica la contraseña contra el hash almacenado.
            try
            {
                if (string.IsNullOrWhiteSpace(usuario.PasswordHash) ||
                    !BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash))
                    throw new UnauthorizedAccessException(
                        "Credenciales incorrectas.");
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch
            {
                // Hash inválido o corrupto: tratar como credenciales incorrectas.
                throw new UnauthorizedAccessException(
                    "Credenciales incorrectas.");
            }

            var token = _tokenService.GenerarToken(usuario);

            LogOperacion("LoginExitoso", $"UsuarioId: {usuario.Id}");

            return new LoginResponse
            {
                Token = token,
                NombreUsuario = usuario.Nombre,
                Rol = usuario.Rol.ToString(),
                Expiracion = DateTime.UtcNow.AddMinutes(60)
            };
        }
    }
}
