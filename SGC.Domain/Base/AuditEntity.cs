namespace SGC.Domain.Base;

public abstract class AuditEntity : EntidadBase
{
    public int? UsuarioId { get; set; }
    public string Accion { get; set; } = string.Empty;
    public string IP { get; set; } = string.Empty;
}