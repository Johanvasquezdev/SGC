using System;

namespace SGC.Domain.Base
{

    public abstract class AuditEntity : EntidadBase
    {
        public string UsuarioId { get; set; } 
        public string Accion { get; set; }
        public string IP { get; set; } 
    }
}