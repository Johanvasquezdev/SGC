using SGC.Domain.Base;

namespace SGC.Domain.Entities.Security
{
    public class Usuario : AuditEntity
    {
        public string Nombre { get; set; } 
        public string Email { get; set; } 
        public string PasswordHash { get; set; } 
    }
}