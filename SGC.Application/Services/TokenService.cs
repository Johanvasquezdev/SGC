using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SGC.Application.Contracts;
using SGC.Domain.Entities.Security;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SGC.Application.Services
{
    // Servicio para generar y validar tokens JWT para autenticacion y autorizacion
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        // Genera un token JWT con los datos del usuario y las configuraciones de seguridad
        public string GenerarToken(Usuario usuario)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var credenciales = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

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

        // Valida un token JWT y retorna el Id del usuario si es valido, o null si no lo es
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
                return int.Parse(jwt.Claims
                    .First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            }
            catch
            {
                return null;
            }
        }
    }
}