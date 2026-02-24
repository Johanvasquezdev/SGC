using SGC.Domain.Base;

namespace SGC.Domain.Entities.Catalog
{
    public sealed class Especialidad : EntidadBase
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool Activo { get; set; } = true;
    }
}