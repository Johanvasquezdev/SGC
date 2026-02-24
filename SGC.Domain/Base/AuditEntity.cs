namespace SGC.Domain.Base
{

    // Esta clase representa una entidad de auditoría que se utilizará para registrar las acciones realizadas en el sistema, como la creación, actualización o eliminación de entidades.
    public class AuditEntity : EntidadBase
    {
        public int UsuarioId { get; set; } // Relación con el usuario que realizó la acción
        public string Entidad { get; set; } = string.Empty; // Nombre de la entidad afectada (e.g., Cita, Paciente, Medico)
        public string Accion { get; set; } = string.Empty; // Acción realizada (e.g., Creación, "Actualización", Eliminación)
        public string? ValorAnterior { get; set; } // Valor anterior de la entidad (puede ser null para acciones de creación)
        public string? ValorNuevo { get; set; } // Valor nuevo de la entidad (puede ser null para acciones de eliminación)
        public DateTime Fecha { get; set; } = DateTime.Now; // Fecha y hora de la acción
        public string DireccionIP { get; set; } = string.Empty; // Dirección IP desde donde se realizó la acción
    }
}