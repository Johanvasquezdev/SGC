using Microsoft.AspNetCore.Mvc;
using SGC.Application.Contracts;
using SGC.Application.DTOs.Security;
using SGC.Domain.Interfaces.Repository;

namespace SGC.API.Controllers
{
    // Controlador para autenticacion de usuarios (login y refresh de tokens)
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;

        public AuthController(IUsuarioRepository usuarioRepository, ITokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
        }

        // POST api/auth/login - Inicia sesion con email y contrasena
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Buscar usuario por email
            var usuario = await _usuarioRepository.GetByEmailAsync(request.Email);
            if (usuario == null)
                return Unauthorized(new { mensaje = "Credenciales incorrectas." });

            // Verificar que el usuario este activo
            if (!usuario.Activo)
                return Unauthorized(new { mensaje = "El usuario esta desactivado." });

            // Verificar contrasena con BCrypt
            if (!BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash))
                return Unauthorized(new { mensaje = "Credenciales incorrectas." });

            // Generar token JWT
            var response = _tokenService.GenerarToken(usuario);
            return Ok(response);
        }

        // POST api/auth/refresh - Renueva el token JWT usando un refresh token
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var response = await _tokenService.RefrescarTokenAsync(request.RefreshToken);
            return Ok(response);
        }
    }
}
