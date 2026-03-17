using SGC.Domain.Entities.Security;

namespace SGC.Application.Contracts
{
    // Servicio para generar y validar tokens JWT de autenticacion
    public interface ITokenService
    {
        string GenerarToken(Usuario usuario);
        int? ValidarToken(string token);
    }
}