using SGC.Domain.Base;

namespace SGC.Domain.Entities.Security
{
    public class Usuario : AuditEntity // La clase Usuario hereda de AuditEntity para incluir propiedades de auditoría como UsuarioId, Entidad, Accion, etc., lo que permite registrar las acciones realizadas por los usuarios en el sistema.
    {
        public string Nombre { get; set; } 
        public string Email { get; set; } 
        public string PasswordHash { get; set; } 
    }
}