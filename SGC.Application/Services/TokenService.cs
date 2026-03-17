using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SGC.Application.Contracts;
using SGC.Application.Services.Base;
using SGC.Domain.Entities.Security;
using SGC.Domain.Interfaces.ILogger;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SGC.Application.Services
{
    // Servicio para generar y validar tokens JWT
    public class TokenService : BaseService, ITokenService
    {
        // Configuración para acceder a los parámetros JWT
        private readonly IConfiguration _config;

        public TokenService(
            IConfiguration config,
            ISGCLogger logger) : base(logger)
        {
            _config = config;
        }

        // Genera un token JWT firmado con los datos del usuario
        public string GenerarToken(Usuario usuario)
        {
            LogOperacion("GenerarToken", $"UsuarioId: {usuario.Id}");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var credenciales = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            // Claims que identifican al usuario dentro del token
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,
                    usuario.Id.ToString()),
                new Claim(ClaimTypes.Email,
                    usuario.Email),
                new Claim(ClaimTypes.Role,
                    usuario.Rol.ToString()),
                new Claim(ClaimTypes.Name,
                    usuario.Nombre)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(_config["Jwt:ExpireMinutes"]!)),
                signingCredentials: credenciales
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Valida un token JWT y retorna el ID del usuario si es válido
        public int? ValidarToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);

                handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _config["Jwt:Audience"],
                    ValidateLifetime = true
                }, out var tokenValidado);

                var jwt = (JwtSecurityToken)tokenValidado;

                // Extrae el ID del usuario desde los claims del token
                return int.Parse(jwt.Claims
                    .First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            }
            catch (Exception ex)
            {
                LogError("ValidarToken", ex);
                return null;
            }
        }
    }
}