using SGC.Domain.Base;

namespace SGC.Domain.Entities.Catalog
{
    // especialidad médica, con propiedades para nombre, descripción y estado de activación. Esta clase es sellada para evitar herencias no deseadas, ya que representa un concepto específico dentro del dominio de la gestión de citas médicas.
    public sealed class Especialidad : EntidadBase
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool Activo { get; set; } = true;
    }
}
