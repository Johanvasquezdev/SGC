using Microsoft.AspNetCore.Mvc;
using SGC.Application.Contracts;
using SGC.Application.DTOs.Security;

namespace SGC.API.Controllers
{
    // Controlador para autenticación de usuarios
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Servicio que maneja la lógica de autenticación
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST api/auth/login - Autentica un usuario y devuelve un token JWT
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest request)
        {
            try
            {
                var response = await _authService.LoginAsync(request); // Intenta autenticar al usuario con las credenciales proporcionadas
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { mensaje = ex.Message });
            }
        }
    }
}