using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SGC.Application.Contracts;
using SGC.Application.DTOs.Security;
using SGC.Domain.Entities.Security;
using SGC.Domain.Interfaces.Repository;

namespace SGC.API.Services
{
    // Implementacion del servicio de tokens JWT para autenticacion
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IUsuarioRepository _usuarioRepository;

        // Almacen en memoria de refresh tokens (en produccion usar base de datos o Redis)
        private static readonly Dictionary<string, int> _refreshTokens = new();

        public TokenService(IConfiguration configuration, IUsuarioRepository usuarioRepository)
        {
            _configuration = configuration;
            _usuarioRepository = usuarioRepository;
        }

        // Genera un token JWT con los claims del usuario y un refresh token
        public LoginResponse GenerarToken(Usuario usuario)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expireMinutes = int.Parse(jwtSettings["ExpireMinutes"] ?? "60");
            var expiracion = DateTime.UtcNow.AddMinutes(expireMinutes);

            // Claims del token: id, email, nombre y rol del usuario
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Role, usuario.Rol.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expiracion,
                signingCredentials: credentials
            );

            // Generar refresh token aleatorio
            var refreshToken = GenerarRefreshToken();
            _refreshTokens[refreshToken] = usuario.Id;

            return new LoginResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiracion = expiracion,
                Usuario = new UsuarioResponse
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Email = usuario.Email,
                    Rol = usuario.Rol.ToString(),
                    FechaCreacion = usuario.FechaCreacion,
                    Activo = usuario.Activo
                }
            };
        }

        // Valida el refresh token y emite un nuevo par JWT + refresh token
        public async Task<LoginResponse> RefrescarTokenAsync(string refreshToken)
        {
            if (!_refreshTokens.TryGetValue(refreshToken, out var usuarioId))
                throw new UnauthorizedAccessException("El refresh token no es valido o ha expirado.");

            // Eliminar el refresh token usado (rotacion de tokens)
            _refreshTokens.Remove(refreshToken);

            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null || !usuario.Activo)
                throw new UnauthorizedAccessException("El usuario no existe o esta desactivado.");

            return GenerarToken(usuario);
        }

        // Genera un string aleatorio seguro para usar como refresh token
        private static string GenerarRefreshToken()
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
