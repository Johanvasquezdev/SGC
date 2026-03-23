namespace SGC.Domain.Interfaces
{
    public interface IAuditable // Interfaz que define las propiedades necesarias para auditar entidades en el sistema. Cualquier entidad que implemente esta interfaz tendrá campos para rastrear cuándo fue creada, cuándo fue modificada por última vez, quién la creó y quién la modificó. Esto es fundamental para mantener un historial de cambios y garantizar la trazabilidad de las acciones realizadas sobre las entidades del sistema.
    {
        DateTime FechaCreacion { get; set; }
        DateTime? FechaModificacion { get; set; }
        string? CreadoPor { get; set; }
        string? ModificadoPor { get; set; }
    }
}