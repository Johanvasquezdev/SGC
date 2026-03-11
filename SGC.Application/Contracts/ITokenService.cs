using SGC.Application.DTOs.Security;
using SGC.Domain.Entities.Security;

namespace SGC.Application.Contracts
{
    // Contrato para la generacion y validacion de tokens JWT
    public interface ITokenService
    {
        // Genera un token JWT y un refresh token para el usuario autenticado
        LoginResponse GenerarToken(Usuario usuario);

        // Valida un refresh token y genera un nuevo par de tokens
        Task<LoginResponse> RefrescarTokenAsync(string refreshToken);
    }
}
